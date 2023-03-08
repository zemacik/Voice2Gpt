using McMaster.Extensions.CommandLineUtils;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Voice2Gpt.App.Infrastructure.Chat;
using Voice2Gpt.App.Infrastructure.Text2Voice;
using Voice2Gpt.App.Infrastructure.Translation;
using Voice2Gpt.App.Infrastructure.Voice2Text;
using Voice2Gpt.App.Infrastructure.VoiceRecorders;

namespace Voice2Gpt.App.ConsoleCommands;

[Command(Name = COMMAND_NAME)]
public class ChatCommand : MyCommandBase
{
    public const string COMMAND_NAME = "chat";

    private readonly IConsole _console;

    public ChatCommand(IConsole console)
        : base(console)
    {
        _console = console;
    }

    [Option("-il|--input-language", Description = "Input language you will speak in (default: en)")]
    private string InputLanguage { get; } = "en";

    [Option("-t|--translate-to-english", Description = "Translate the message to English before sending it to the chatbot")]
    private bool TranslateToEnglishAndBack { get; } = false;

    [Option("-d|--device", Description = "Microphone device number.")]
    private int? DeviceNo { get; } = null;

    public System.ComponentModel.DataAnnotations.ValidationResult OnValidate()
    {
        return System.ComponentModel.DataAnnotations.ValidationResult.Success!;
    }

    protected async Task OnExecute()
    {
        ShowHeader();

        Console.WriteLine("Press Ctrl+C to exit");

        var serviceProvider = BuildServiceProvider();

        using var cts = new CancellationTokenSource();
        using var scope = serviceProvider.CreateScope();

        _console.CancelKeyPress += (_, e) =>
        {
            _console.WriteLine("Canceling...");
            e.Cancel = true;
            cts.Cancel();
        };

        var tsSvc = serviceProvider.GetRequiredService<Voice2GptService>();

        await tsSvc.Start(
            DeviceNo,
            TranslateToEnglishAndBack,
            InputLanguage,
            _console,
            cts.Token);
    }

    protected override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        ConfigureBaseServices(services, configuration);

        services.AddScoped<Voice2GptService>(); // this is the service that will be used to start the conversation

        services.AddScoped<IVoiceRecorder>(_ => new NAudioVoiceRecorder(new NAudioVoiceRecorderOptions
        {
            //OutputFolterPath = @"c:\temp\",
            ShowAudioLevelMeter = true,
            ShowTimeElapsed = true,
        }));

        services.AddScoped<ITranscriber>(_ => new OpenAiWhisperTranscriber(new OpenAiWhisperTranscriberOptions
        {
            ApiKey = configuration["OpenAIServiceOptions:ApiKey"] ??
                     throw new Exception("Missing OpenAIServiceOptions:ApiKey in appsettings.json"),
            OrganizationId = configuration["OpenAIServiceOptions:OrgKey"] ??
                             throw new Exception("missing OpenAIServiceOptions:OrgKey in appsettings.json"),
            Language = InputLanguage,
        }));

        services.AddScoped<IChatService>(_ => new ChatGptChatService(new ChatGptServiceOptions
        {
            ApiKey = configuration["OpenAIServiceOptions:ApiKey"] ??
                     throw new Exception("missing OpenAIServiceOptions:ApiKey in appsettings.json"),
            OrganizationId = configuration["OpenAIServiceOptions:OrgKey"] ??
                             throw new Exception("missing OpenAIServiceOptions:OrgKey in appsettings.json"),

            Language = InputLanguage,
            DefinitionOfChatbotAssistant = "You are a helpful assistant.", // initial definition of the chatbot assistant
        }));

        services.AddScoped<ITranslator>(_ => new DeepLTranslator(new DeepLTranslatorOptions
        {
            ApiKey = configuration["DeepLTranslatorOptions:ApiKey"] ??
                     throw new Exception("Missing DeepLTranslatorOptions:ApiKey in appsettings.json"),
        }));

        services.AddScoped<ISpeechSynthesizer>(provider =>
            new MsWindowsSpeechSynthesizer(
                new SpeechSynthesizerOptions { Language = InputLanguage },
                provider.GetRequiredService<ILogger<MsWindowsSpeechSynthesizer>>())
        );
    }
}
