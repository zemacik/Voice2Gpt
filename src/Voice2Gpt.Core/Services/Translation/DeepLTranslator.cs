namespace Voice2Gpt.App.Infrastructure.Translation;

/// <summary>
/// DeepL translator service.
/// https://www.deepl.com/translator
/// </summary>
public class DeepLTranslator : ITranslator
{
    private readonly DeepLTranslatorOptions _options;

    /// <summary>
    /// Creates a new instance of the <see cref="DeepLTranslator"/> class.
    /// </summary>
    /// <param name="options">The options for the translator.</param>
    public DeepLTranslator(DeepLTranslatorOptions options)
    {
        _options = options;
    }

    /// <inheritdoc />
    public Task<string> Translate(string text, string sourceLanguage, string targetLanguage)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// The options for the <see cref="DeepLTranslator"/>.
/// </summary>
public class DeepLTranslatorOptions
{
    /// <summary>
    /// The API key for the DeepL API.
    /// </summary>
    public string ApiKey { get; init; } = null!;
}
