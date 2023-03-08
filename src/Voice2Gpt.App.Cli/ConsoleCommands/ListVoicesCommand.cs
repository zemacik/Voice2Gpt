using McMaster.Extensions.CommandLineUtils;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Voice2Gpt.App.Infrastructure.Text2Voice;
using Voice2Gpt.App.Infrastructure.VoiceRecorders;

namespace Voice2Gpt.App.ConsoleCommands;

[Command(Name = COMMAND_NAME)]
public class ListVoicesCommand : MyCommandBase
{
    public const string COMMAND_NAME = "list-voices";

    private readonly IConsole _console;

    public ListVoicesCommand(IConsole console)
        : base(console)
    {
        _console = console;
    }

    protected void OnExecute()
    {
        ShowHeader();

        var serviceProvider = BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();

        var voiceRecorder = serviceProvider.GetRequiredService<ISpeechSynthesizer>();
        foreach (var voice in voiceRecorder.ListVoices())
        {
            _console.WriteLine($"{voice.VoiceNo}: {voice.Description}");
        }
    }

    protected override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        ConfigureBaseServices(services, configuration);

        services.AddScoped<ISpeechSynthesizer>(provider =>
            new MsWindowsSpeechSynthesizer(new SpeechSynthesizerOptions(),
                provider.GetRequiredService<ILogger<MsWindowsSpeechSynthesizer>>()));
    }
}
