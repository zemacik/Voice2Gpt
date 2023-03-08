using System.Globalization;
using System.Speech.Synthesis;

using Microsoft.Extensions.Logging;

namespace Voice2Gpt.App.Infrastructure.Text2Voice;

/// <summary>
/// The native Microsoft Windows speech synthesizer.
/// Note: Works only on Windows platform.
/// </summary>
public class MsWindowsSpeechSynthesizer : ISpeechSynthesizer
{
    private readonly ILogger<MsWindowsSpeechSynthesizer> _logger;
    private readonly SpeechSynthesizerOptions _options;

    /// <summary>
    /// Creates a new instance of the <see cref="MsWindowsSpeechSynthesizer"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <param name="logger">The logger.</param>
    public MsWindowsSpeechSynthesizer(SpeechSynthesizerOptions options,
        ILogger<MsWindowsSpeechSynthesizer> logger)
    {
        _logger = logger;
        _options = options ?? new SpeechSynthesizerOptions
        {
            Language = "en",
        };
    }

    /// <inheritdoc />
    public Task Speak(string text)
    {
#pragma warning disable CA1416 // sais that the API is not supported on the other platform than Windows
        var synth = new SpeechSynthesizer();

        synth.InjectOneCoreVoices();
        synth.SelectVoiceByHints(VoiceGender.Male, VoiceAge.Adult, 0, new CultureInfo(_options.Language));

        _logger.LogInformation("Selected voice for speech synthesizer by hint language \'{OptionsLanguage}\':",
            _options.Language);

        _logger.LogInformation("Name: {VoiceName}, Culture: {VoiceCulture}, Gender: {VoiceGender}, Age: {VoiceAge}",
            synth.Voice.Name,
            synth.Voice.Culture,
            synth.Voice.Gender,
            synth.Voice.Age);

        synth.SetOutputToDefaultAudioDevice();
        synth.Speak(text);

        return Task.CompletedTask;
#pragma warning restore CA1416
    }

    /// <inheritdoc />
    public IEnumerable<VoiceInfo> ListVoices()
    {
        var synth = new SpeechSynthesizer();
        synth.InjectOneCoreVoices();

        var voiceIndex = 0;
        foreach (var voice in synth.GetInstalledVoices())
        {
            var info = voice.VoiceInfo;

            yield return new VoiceInfo
            {
                VoiceNo = voiceIndex++,
                Description = $"Name: {info.Name}, Culture: {info.Culture}, Gender: {info.Gender}, Age: {info.Age}"
            };
        }
    }
}
