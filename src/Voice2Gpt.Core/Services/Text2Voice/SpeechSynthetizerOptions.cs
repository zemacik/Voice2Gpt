namespace Voice2Gpt.App.Infrastructure.Text2Voice;

/// <summary>
/// The options for the speech synthesizer.
/// </summary>
public class SpeechSynthesizerOptions
{
    /// <summary>
    /// The language the synthesizer should use.
    /// </summary>
    public string Language { get; init; } = "en";
}
