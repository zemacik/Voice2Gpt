using System.Diagnostics;

using NAudio.Lame;
using NAudio.Wave;

namespace Voice2Gpt.App.Infrastructure.VoiceRecorders;

/// <summary>
/// The NAudio voice recorder.
/// </summary>
public class NAudioVoiceRecorder : IVoiceRecorder
{
    private readonly NAudioVoiceRecorderOptions _options;

    /// <summary>
    /// Creates a new instance of <see cref="NAudioVoiceRecorder"/>
    /// </summary>
    /// <param name="options">The options to use.</param>
    public NAudioVoiceRecorder(NAudioVoiceRecorderOptions options)
    {
        _options = options;
    }

    /// <inheritdoc />
    public async Task<string> Record(int deviceNumber, CancellationToken cancellationToken)
    {
        Console.WriteLine();
        Console.WriteLine("Press any key to start recording.");
        Console.ReadKey();

        // Create a new instance of the WaveInEvent class, which represents an audio input device (i.e., microphone)
        using var waveIn = new WaveInEvent
        {
            DeviceNumber = deviceNumber
        };

        // Set the audio input device's sample rate, number of channels, and bit depth
        waveIn.WaveFormat = new WaveFormat(44100, NAudio.Wave.WaveInEvent.GetCapabilities(deviceNumber).Channels);

        // the file name to record to
        var recordingFileName = Path.Combine(_options.OutputFolterPath ?? Path.GetTempPath(),
            $"recording_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.mp3");

        // Set up an event handler to handle the data that is recorded from the microphone
        await using var mp3Writer = new LameMP3FileWriter(recordingFileName, waveIn.WaveFormat, LAMEPreset.VBR_90);

        var timer = new Stopwatch();

        waveIn.DataAvailable += (_, e) =>
        {
            if (cancellationToken.IsCancellationRequested)
            {
                waveIn.StopRecording();

                return;
            }

            mp3Writer.Write(e.Buffer, 0, e.BytesRecorded);

            DisplayInfo(e, timer);
        };

        // Start recording audio from the microphone
        waveIn.StartRecording();
        timer.Start();

        await Task.Delay(400, cancellationToken);

        // Wait for the user to press a key to stop recording
        Console.WriteLine("Recording... Press any key to stop.");
        Console.ReadKey();

        // Stop recording audio from the microphone
        waveIn.StopRecording();
        timer.Stop();

        return recordingFileName;
    }

    /// <inheritdoc />
    public IEnumerable<DeviceInfo> ListDevices()
    {
        for (var i = 0; i < WaveInEvent.DeviceCount; i++)
        {
            var capabilities = WaveInEvent.GetCapabilities(i);

            yield return new DeviceInfo
            {
                DeviceNo = i,
                ProductName = capabilities.ProductName,
                Channels = capabilities.Channels,
                ProductGuid = capabilities.ProductGuid,
                ManufacturerGuid = capabilities.ManufacturerGuid,
            };
        }
    }

    /// <summary>
    /// Displays the time elapsed and the audio level meter.
    /// </summary>
    /// <param name="waveInEventArgs">The audio data received event data.</param>
    /// <param name="timer">The current timer.</param>
    private void DisplayInfo(WaveInEventArgs waveInEventArgs, Stopwatch timer)
    {
        string? time = null;
        string? audioLevel = null;

        if (_options.ShowTimeElapsed)
        {
            time = GetTimeElapsedString(timer);
        }

        if (_options.ShowAudioLevelMeter)
        {
            audioLevel = GetAudioLevelMeterString(waveInEventArgs.Buffer);
        }

        Console.CursorLeft = 0;
        Console.CursorVisible = false;
        Console.Write($"{time} {audioLevel}");
    }

    /// <summary>
    /// Creates a string that represents the time elapsed.
    /// </summary>
    /// <param name="timer">The current timer.</param>
    /// <returns>The time elapsed string.</returns>
    private string GetTimeElapsedString(Stopwatch timer)
    {
        var time = timer.Elapsed;

        return $"{time:hh\\:mm\\:ss}";
    }

    /// <summary>
    /// Creates a string that represents the audio level meter.
    /// </summary>
    /// <param name="eBuffer">The data buffer receiveid from the microphone.</param>
    /// <returns>The audio level meter string.</returns>
    private string GetAudioLevelMeterString(byte[] eBuffer)
    {
        // copy buffer into an array of integers
        short[] values = new short[eBuffer.Length / 2];
        Buffer.BlockCopy(eBuffer, 0, values, 0, eBuffer.Length);

        // determine the highest value as a fraction of the maximum possible value
        var fraction = (float)values.Max() / 32768;

        // print a level meter using the console
        string bar = new('#', (int)(fraction * 70));
        var meter = "[" + bar.PadRight(60, '-') + "]";

        return $"{meter} {fraction * 100:00.0}%";
    }
}

/// <summary>
/// The options for the <see cref="NAudioVoiceRecorder"/>
/// </summary>
public class NAudioVoiceRecorderOptions
{
    /// <summary>
    /// The path to the output folder.
    /// </summary>
    public string? OutputFolterPath { get; init; }

    /// <summary>
    /// The flag to indicate if the audio level meter should be shown.
    /// </summary>
    public bool ShowAudioLevelMeter { get; init; }

    /// <summary>
    /// The flag to indicate if the time elapsed should be shown.
    /// </summary>
    public bool ShowTimeElapsed { get; init; }
}

/// <summary>
/// The audio device information.
/// </summary>
public class DeviceInfo
{
    public int DeviceNo { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public int Channels { get; init; }
    public Guid ProductGuid { get; init; }
    public Guid ManufacturerGuid { get; init; }
}
