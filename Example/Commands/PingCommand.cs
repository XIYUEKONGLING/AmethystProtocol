using Example.Network;
using Example.Settings;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Example.Commands;

public class PingCommand : AsyncCommand<PingSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, PingSettings settings, CancellationToken cancellationToken)
    {
        AnsiConsole.MarkupLine($"Pinging [bold]{settings.Host}:{settings.Port}[/]...");
        AnsiConsole.MarkupLine("[grey]Press Ctrl+C to stop[/]");
        Console.WriteLine();

        var count = 0;
        
        try
        {
            while (settings.Count == null || count < settings.Count)
            {
                await PingOnceAsync(settings.Host, settings.Port, count + 1);
                count++;

                if (settings.Count == null || count < settings.Count)
                {
                    await Task.Delay(settings.Interval);
                }
            }
        }
        catch (TaskCanceledException)
        {
            // Graceful exit
        }

        return 0;
    }

    private static async Task PingOnceAsync(string host, int port, int sequence)
    {
        try
        {
            // Note: We create a new connection for each ping because standard
            // servers often close the connection after the Status/Ping cycle.
            using var client = new ProtocolClient(host, port);
            
            await client.ConnectAsync();
            client.PerformHandshake();
            
            // We must request status first to be compliant with most server implementations
            // before sending the ping packet, even if we ignore the status body.
            client.RequestStatus(); 
            
            var latency = client.PerformPing();

            var color = latency < 100 ? "green" : latency < 300 ? "yellow" : "red";
            AnsiConsole.MarkupLine($"Seq=[blue]{sequence}[/] Time=[{color}]{latency}ms[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"Seq=[blue]{sequence}[/] [red]Failed: {ex.Message}[/]");
        }
    }
}
