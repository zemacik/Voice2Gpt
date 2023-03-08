using McMaster.Extensions.CommandLineUtils;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Voice2Gpt.App.Infrastructure.VoiceRecorders;

namespace Voice2Gpt.App.ConsoleCommands;

[Command(Name = COMMAND_NAME)]
public class ListDevicesCommand : MyCommandBase
{
    public const string COMMAND_NAME = "list-devices";

    private readonly IConsole _console;

    public ListDevicesCommand(IConsole console)
        : base(console)
    {
        _console = console;
    }

    protected void OnExecute()
    {
        ShowHeader();

        var serviceProvider = BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();

        var voiceRecorder = serviceProvider.GetRequiredService<IVoiceRecorder>();
        foreach (var device in voiceRecorder.ListDevices())
        {
            _console.WriteLine($"{device.DeviceNo}: {device.ProductName}");
        }
    }

    protected override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        ConfigureBaseServices(services, configuration);

        services.AddScoped<IVoiceRecorder>(_ => new NAudioVoiceRecorder(new NAudioVoiceRecorderOptions()));
    }
}
