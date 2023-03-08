// Copyright (c) Michal Krchnavy. All rights reserved.
// Licensed under the MIT license.See LICENSE file in the project root for full license information.

namespace Voice2Gpt.Core.Models;

/// <summary>
/// The context of the previous iterations.
/// </summary>
public class IterationsContext
{
    public List<ChatPrompt> ChatPrompts { get; init; } = new();
}

/// <summary>
/// The chat prompt.
/// </summary>
/// <param name="Speaker">The speaker of the message. (e.g. system, user, chat)</param>
/// <param name="Message"></param>
public record ChatPrompt(string Speaker, string Message);
