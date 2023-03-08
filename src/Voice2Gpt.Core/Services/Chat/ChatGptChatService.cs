using OpenAI;
using OpenAI.Chat;

using Voice2Gpt.Core.Models;

namespace Voice2Gpt.App.Infrastructure.Chat;

/// <summary>
/// Chat service using OpenAI Chat API.
/// </summary>
public class ChatGptChatService : IChatService
{
    const string DEFINITION_OF_CHATBOT_ASSISTANT = "You are a helpful assistant.";
    private readonly ChatGptServiceOptions _options;

    /// <summary>
    /// Creates a new instance of <see cref="ChatGptChatService"/>.
    /// </summary>
    /// <param name="options">The options for the service.</param>
    public ChatGptChatService(ChatGptServiceOptions options)
    {
        _options = options;
    }

    /// <inheritdoc/>
    public async Task<string> Tell(string message, IterationsContext context)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException($"'{nameof(message)}' cannot be null or whitespace.", nameof(message));
        }

        var api = new OpenAIClient(
            new OpenAIAuthentication(_options.ApiKey, _options.OrganizationId));

        var chatPrompts = new List<OpenAI.Chat.ChatPrompt>();

        chatPrompts.Add(new OpenAI.Chat.ChatPrompt("system",
            _options.DefinitionOfChatbotAssistant ?? DEFINITION_OF_CHATBOT_ASSISTANT));

        // add the previous messages to the chat prompts
        context.ChatPrompts.ForEach(x => chatPrompts.Add(new OpenAI.Chat.ChatPrompt(x.Speaker, x.Message)));

        // add the new message to the chat prompts
        chatPrompts.Add(new OpenAI.Chat.ChatPrompt("user", message));

        var chatRequest = new ChatRequest(chatPrompts);
        var result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);

        var response = result.FirstChoice;

        return response;
    }
}

/// <summary>
/// The options for the <see cref="ChatGptService"/>.
/// </summary>
public class ChatGptServiceOptions
{
    /// <summary>
    /// The API key for the OpenAI API.
    /// </summary>
    public string ApiKey { get; init; } = null!;

    /// <summary>
    /// The organization ID for the OpenAI API.
    /// </summary>
    public string OrganizationId { get; init; } = null!;

    /// <summary>
    /// The language to use for the chatbot assistant.
    /// </summary>
    public string? Language { get; init; }

    /// <summary>
    /// The definition of the chatbot assistant.
    /// The text that will configure the chatbot assistant before the first interaction.
    /// e.g. "You are a helpful assistant."
    /// </summary>
    public string? DefinitionOfChatbotAssistant { get; init; }
}
