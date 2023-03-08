using System.Net.Http.Headers;

namespace Voice2Gpt.App.Infrastructure.Voice2Text;

/// <summary>
/// OpenAi Whisper Transcriber
/// Uses the OpenAi API Whisper AI service to transcribe audio to text.
/// </summary>
public class OpenAiWhisperTranscriber : ITranscriber
{
    private readonly OpenAiWhisperTranscriberOptions _options;

    /// <summary>
    /// Creates a new instance of <see cref="OpenAiWhisperTranscriber"/>
    /// </summary>
    /// <param name="options">The options</param>
    public OpenAiWhisperTranscriber(OpenAiWhisperTranscriberOptions options)
    {
        _options = options;
    }

    /// <inheritdoc />
    public async Task<string> Transcribe(string audioFilePath)
    {
        using var httpClient = new HttpClient();

        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _options.ApiKey);

        httpClient.DefaultRequestHeaders.Add("OpenAI-Organization", _options.OrganizationId);
        using var form = new MultipartFormDataContent();

        form.Add(new StringContent("whisper-1"), "model");
        form.Add(new StringContent("text"), "response_format");

        if (_options.Language != null)
            form.Add(new StringContent(_options.Language), "language");

        await using var fileStream = new FileStream(audioFilePath, FileMode.Open);
        using var fileContent = new StreamContent(fileStream);
        form.Add(fileContent, name: "file", fileName: "openai.mp3");

        using var response = await httpClient.PostAsync("https://api.openai.com/v1/audio/transcriptions", form);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();

        return responseBody;
    }
}

/// <summary>
/// Options for the <see cref="OpenAiWhisperTranscriber"/>
/// </summary>
public class OpenAiWhisperTranscriberOptions
{
    public string ApiKey { get; init; } = null!;
    public string OrganizationId { get; init; } = null!;
    public string? Language { get; init; }
}

