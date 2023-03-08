using McMaster.Extensions.CommandLineUtils;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Voice2Gpt.App.ConsoleCommands;

public abstract class MyCommandBase
{
    private readonly IConsole _console;

    [Option("-l|--log", Description = "Enable logging")]
    private bool EnableLogging { get; } = false;

    public MyCommandBase(IConsole console)
    {
        _console = console;
    }

    protected IServiceProvider BuildServiceProvider()
    {
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile($"appsettings.{environmentName}.json", true)
            .AddEnvironmentVariables();

        var config = builder.Build();

        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection, config);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        return serviceProvider;
    }

    protected abstract void ConfigureServices(IServiceCollection services, IConfiguration configuration);

    protected void ConfigureBaseServices(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddLogging(configure =>
        {
            configure.ClearProviders();
            configure.AddConfiguration(configuration.GetSection("Logging"));
            configure.AddDebug();

            // enable logging only if command line argument is set
            if (EnableLogging)
            {
                configure.AddConsole();
            }
        });

        services.AddScoped<IConfiguration>(_ => configuration);
    }

    protected void ShowHeader()
    {
        _console.WriteLine("Voice2Gpt.App.Cli");
        _console.WriteLine("Start with --help to see all available configuration options.");
        _console.WriteLine();
    }
}
