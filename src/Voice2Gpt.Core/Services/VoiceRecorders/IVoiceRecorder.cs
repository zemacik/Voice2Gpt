namespace Voice2Gpt.App.Infrastructure.VoiceRecorders;

/// <summary>
/// The voice recorder interface.
/// </summary>
public interface IVoiceRecorder
{
    /// <summary>
    /// Records voice from the specified device.
    /// </summary>
    /// <param name="deviceNumber">The device number.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The path to the recorded file.</returns>
    Task<string> Record(int deviceNumber, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the list of available devices.
    /// </summary>
    /// <returns>The list of available devices.</returns>
    IEnumerable<DeviceInfo> ListDevices();
}
