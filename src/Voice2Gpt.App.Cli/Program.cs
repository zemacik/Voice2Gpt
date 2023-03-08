using McMaster.Extensions.CommandLineUtils;

using Voice2Gpt.App.ConsoleCommands;

namespace Voice2Gpt.App;

[HelpOption("-?| -h | --help")]
[Command(Name = "voice2gpt"),
 Subcommand(typeof(ChatCommand), typeof(ListDevicesCommand), typeof(ListVoicesCommand))]
internal class Program
{
    private const string DEFAULT_COMMAND = ChatCommand.COMMAND_NAME;

    public static async Task Main(string[] args)
        => await CommandLineApplication.ExecuteAsync<Program>(args);

    private Task<int> OnExecute(CommandLineApplication app)
    {
        var defaultCommand = app.Commands.First(x => x.Name == DEFAULT_COMMAND);

        return defaultCommand.ExecuteAsync(app.RemainingArguments.ToArray());
    }
}
