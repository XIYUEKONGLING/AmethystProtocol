using Example.Commands;
using Spectre.Console.Cli;

namespace Example;

public static class Program
{
    public static int Main(string[] args)
    {
        var app = new CommandApp();

        app.Configure(config =>
        {
            config.SetApplicationName("Example");

            config.AddCommand<StatusCommand>("status")
                .WithDescription("Get server status and information.");

            config.AddCommand<PingCommand>("ping")
                .WithDescription("Continuously ping the server to measure latency.");
        });

        return app.Run(args);
    }
}