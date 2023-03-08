namespace Voice2Gpt.App.Infrastructure.Translation;

/// <summary>
/// The ChatGPT translator service.
/// </summary>
public class ChatGptTranslator : ITranslator
{
    private readonly ChatGptTranslatorOptions _options;

    /// <summary>
    /// Creates a new instance of the <see cref="ChatGptTranslator"/>.
    /// </summary>
    /// <param name="options">The options for the <see cref="ChatGptTranslator"/>.</param>
    public ChatGptTranslator(ChatGptTranslatorOptions options)
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
/// The options for the <see cref="ChatGptTranslator"/>.
/// </summary>
public class ChatGptTranslatorOptions
{
    /// <summary>
    /// The API key for the ChatGPT API.
    /// </summary>
    public string ApiKey { get; init; } = null!;

    /// <summary>
    /// The organization ID for the ChatGPT API.
    /// </summary>
    public string OrganizationId { get; init; } = null!;
}
