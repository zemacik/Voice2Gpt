using Microsoft.Extensions.Logging;

namespace Voice2Gpt.App.Infrastructure.Text2Voice;

/// <summary>
/// Azure Cognitive Services Text to Speech service.
/// </summary>
public class AzureCognitiveServicesText2Speech : ISpeechSynthesizer
{
    private readonly ILogger<AzureCognitiveServicesText2Speech> _logger;
    private readonly SpeechSynthesizerOptions _options;

    /// <summary>
    /// Creates a new instance of the <see cref="AzureCognitiveServicesText2Speech"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <param name="logger">The logger.</param>
    public AzureCognitiveServicesText2Speech(SpeechSynthesizerOptions? options,
        ILogger<AzureCognitiveServicesText2Speech> logger)
    {
        _logger = logger;
        _options = options ?? new SpeechSynthesizerOptions
        {
            Language = "en",
        };
    }

    /// <inheritdoc />
    public Task Speak(string text)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public IEnumerable<VoiceInfo> ListVoices()
    {
        throw new NotImplementedException();
    }
}
