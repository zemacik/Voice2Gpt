namespace Voice2Gpt.App.Infrastructure.Voice2Text;

/// <summary>
/// The transcriber service interface.
/// Transcribes audio to text.
/// </summary>
public interface ITranscriber
{
    /// <summary>
    /// Transcribes audio to text.
    /// </summary>
    /// <param name="audioFilePath">The path to the audio file.</param>
    /// <returns>The text transcription.</returns>
    Task<string> Transcribe(string audioFilePath);
}
