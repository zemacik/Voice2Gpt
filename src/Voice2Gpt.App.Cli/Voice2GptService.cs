using McMaster.Extensions.CommandLineUtils;

using Microsoft.Extensions.Logging;

using Voice2Gpt.App.Infrastructure.Chat;
using Voice2Gpt.App.Infrastructure.Text2Voice;
using Voice2Gpt.App.Infrastructure.Translation;
using Voice2Gpt.App.Infrastructure.Voice2Text;
using Voice2Gpt.App.Infrastructure.VoiceRecorders;
using Voice2Gpt.Core.Models;

namespace Voice2Gpt.App;

/// <summary>
/// Service encapsulating the main listening, recording, transcribing, translating, ....
/// </summary>
internal class Voice2GptService
{
    private readonly IVoiceRecorder _voiceRecorder;
    private readonly ITranscriber _transcriber;
    private readonly ITranslator _translator;
    private readonly IChatService _chatService;
    private readonly ISpeechSynthesizer _voiceSynthesizer;
    private readonly ILogger<Voice2GptService> _logger;

    private int _deviceNo;

    /// <summary>
    /// Creates a new instance of the <see cref="Voice2GptService"/> class.
    /// </summary>
    public Voice2GptService(
        IVoiceRecorder voiceRecorder,
        ITranscriber transcriber,
        ITranslator translator,
        IChatService chatService,
        ISpeechSynthesizer voiceSynthesizer,
        ILogger<Voice2GptService> logger)
    {
        _voiceRecorder = voiceRecorder;
        _transcriber = transcriber;
        _translator = translator;
        _chatService = chatService;
        _voiceSynthesizer = voiceSynthesizer;
        _logger = logger;
    }

    /// <summary>
    /// Starts the process of recording, transcribing, translating, sending to chat service and playing the response.
    /// </summary>
    /// <param name="deviceNo">The device number of the microphone to use.</param>
    /// <param name="translateToEnglish">If true, the message will be translated to English before sending it to the chat service.</param>
    /// <param name="lang">The language the user is speaking in.</param>
    /// <param name="console">The console to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task Start(int? deviceNo,
        bool translateToEnglish,
        string lang,
        IConsole console,
        CancellationToken cancellationToken)
    {
        IterationsContext context = new();

        ValidateAndSelectDevice(deviceNo, console);

        while (!cancellationToken.IsCancellationRequested)
        {
            context = await StartItteration(translateToEnglish, lang, context, cancellationToken);
        }
    }

    /// <summary>
    /// Starts the current iteration of the process.
    /// The iterration is defined as one recording, transcription, translation, chat service request and response.
    /// </summary>
    /// <param name="translateToEnglish">If true, the message will be translated to English before sending it to the chat service.</param>
    /// <param name="lang">The language the user is speaking in.</param>
    /// <param name="previousIterationsContext">The context of the previous iterrations.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    private async Task<IterationsContext> StartItteration(
        bool translateToEnglish,
        string lang,
        IterationsContext previousIterationsContext,
        CancellationToken cancellationToken)
    {
        // TODO: cancellationToken handling

        var audioFilePath = await _voiceRecorder.Record(_deviceNo, cancellationToken);

        _logger.LogInformation("Recording finished. Audio file saved to {AudioFilePath}", audioFilePath);

        _logger.LogInformation("Transcribing audio file {AudioFilePath}", audioFilePath);

        // Transcribe the audio file to text
        var saidMessage = await _transcriber.Transcribe(audioFilePath);

        _logger.LogInformation("Transcription finished. Message: {SaidMessage}", saidMessage);

        var messageToChat = saidMessage;

        // if Translation is enabled, translate the question to English
        if (translateToEnglish && lang != "en")
        {
            _logger.LogInformation("Translating message to English");
            messageToChat = await _translator.Translate(saidMessage, sourceLanguage: lang, targetLanguage: "en");
        }

        _logger.LogInformation("Sending message to chat service: {MessageToChat}", messageToChat);

        // Send the message to the chat service
        var response = await _chatService.Tell(messageToChat, previousIterationsContext);

        // Add the new message to the chat prompts and create a new context
        var newIterationContext = new IterationsContext
        {
            ChatPrompts = new List<ChatPrompt>(previousIterationsContext.ChatPrompts)
            {
                new("user", messageToChat),
                new("assistant", response)
            }
        };

        var textToSpeak = response;

        // if Translation is enabled, translate the response back to the original language
        if (translateToEnglish && lang != "en")
        {
            _logger.LogInformation("Translating response to {Lang}", lang);
            textToSpeak = await _translator.Translate(response, sourceLanguage: "en", targetLanguage: lang);
        }

        _logger.LogInformation("Speaking response: {TextToSpeak}", textToSpeak);

        // Speak the response
        await _voiceSynthesizer.Speak(textToSpeak);

        _logger.LogInformation("Finished");

        return newIterationContext;
    }

    /// <summary>
    /// Validates the device number from the input and if it is not provided, selects the first microphone device.
    /// </summary>
    /// <param name="deviceNo">The device number from the input.</param>
    /// <param name="console">The console to use.</param>
    private void ValidateAndSelectDevice(int? deviceNo, IConsole console)
    {
        var devices = _voiceRecorder.ListDevices().ToList();

        if (!deviceNo.HasValue)
        {
            var device = devices
                .Where(i => i.ProductName.ToLowerInvariant().Contains("microphone"))
                .Select(i => i)
                .FirstOrDefault();

            if (device == null)
            {
                throw new Exception("No microphone devices found");
            }

            console.WriteLine("No device number provided.");
            console.WriteLine($"Using: {device.DeviceNo} - {device.ProductName}");

            deviceNo = device.DeviceNo;
        }

        if (deviceNo.Value >= devices.Count)
        {
            throw new Exception($"Device number {deviceNo.Value} is out of range");
        }

        _deviceNo = deviceNo.Value;
    }
}


