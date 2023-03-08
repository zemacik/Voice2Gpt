using Microsoft.Extensions.Logging;

namespace Voice2Gpt.App.Infrastructure.Text2Voice;

/// <summary>
/// ElevenLabs.ai Text to Speech service.
/// https://beta.elevenlabs.io/
/// </summary>
public class ElevenLabsAiText2Speech : ISpeechSynthesizer
{
    private readonly ILogger<ElevenLabsAiText2Speech> _logger;
    private readonly SpeechSynthesizerOptions _options;

    /// <summary>
    /// Creates a new instance of the <see cref="ElevenLabsAiText2Speech"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <param name="logger">The logger.</param>
    public ElevenLabsAiText2Speech(SpeechSynthesizerOptions? options,
        ILogger<ElevenLabsAiText2Speech> logger)
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
