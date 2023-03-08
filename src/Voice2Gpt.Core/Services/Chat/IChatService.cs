using Voice2Gpt.Core.Models;

namespace Voice2Gpt.App.Infrastructure.Chat;

/// <summary>
/// The chat service.
/// </summary>
public interface IChatService
{
    /// <summary>
    /// Send a message to the chat service.
    /// </summary>
    /// <param name="message">Message to send.</param>
    /// <param name="context">The previous iterations context.</param>
    /// <returns>Response from the chat service.</returns>
    Task<string> Tell(string message, IterationsContext context);
}
