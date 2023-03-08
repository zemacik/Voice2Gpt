namespace Voice2Gpt.App.Infrastructure.Text2Voice;

/// <summary>
/// The interface for a speech synthesizer.
/// Speech synthesizers are used to convert text to speech.
/// </summary>
public interface ISpeechSynthesizer
{
    /// <summary>
    /// Speaks the given text.
    /// </summary>
    /// <param name="text">The text to speak.</param>
    Task Speak(string text);

    /// <summary>
    /// Gets all the voices available.
    /// </summary>
    /// <returns>The available voices.</returns>
    IEnumerable<VoiceInfo> ListVoices();
}

/// <summary>
/// The information about a voice.
/// </summary>
public class VoiceInfo
{
    /// <summary>
    /// The voice number.
    /// </summary>
    public int VoiceNo { get; init; }

    /// <summary>
    /// The voice description.
    /// </summary>
    public string Description { get; init; } = null!;
}
