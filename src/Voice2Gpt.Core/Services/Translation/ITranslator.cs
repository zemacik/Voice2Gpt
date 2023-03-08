namespace Voice2Gpt.App.Infrastructure.Translation;

/// <summary>
/// The translator service interface.
/// </summary>
public interface ITranslator
{
    /// <summary>
    /// Translates the given text to the target language.
    /// </summary>
    /// <param name="text">The text to translate.</param>
    /// <param name="sourceLanguage">The language of the text.</param>
    /// <param name="targetLanguage">The language to translate the text to.</param>
    /// <returns>The translated text.</returns>
    Task<string> Translate(string text, string sourceLanguage, string targetLanguage);
}
