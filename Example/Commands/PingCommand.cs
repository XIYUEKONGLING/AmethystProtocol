using Example.Network;
using Example.Settings;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Example.Commands;

public class PingCommand : AsyncCommand<PingSettings>
{
    private class PingStats
    {
        public int Sent { get; set; }
        public int Received { get; set; }
        public long Min { get; set; } = long.MaxValue;
        public long Max { get; set; } = long.MinValue;
        public long Sum { get; set; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, PingSettings settings, CancellationToken cancellationToken)
    {
        // Spectre.Console handles Ctrl+C by cancelling the token passed to the command,
        // but since we want to print stats on exit, we handle the loop manually.
        
        AnsiConsole.MarkupLine($"Pinging [bold]{settings.Host}:{settings.Port}[/]...");
        AnsiConsole.MarkupLine("[grey]Press Ctrl+C to stop[/]");
        Console.WriteLine();

        var stats = new PingStats();
        var sequence = 0;

        try
        {
            // Use a CancellationTokenSource linked to console cancel to gracefully stop
            using var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) => {
                e.Cancel = true; // Prevent immediate termination
                cts.Cancel();
            };

            while (!cts.Token.IsCancellationRequested)
            {
                if (settings.Count.HasValue && sequence >= settings.Count.Value)
                {
                    break;
                }

                sequence++;
                stats.Sent++;

                await PingOnceAsync(settings.Host, settings.Port, sequence, stats);

                if (!cts.Token.IsCancellationRequested && 
                    (!settings.Count.HasValue || sequence < settings.Count.Value))
                {
                    try
                    {
                        await Task.Delay(settings.Interval, cts.Token);
                    }
                    catch (TaskCanceledException)
                    {
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error:[/ {Markup.Escape(ex.Message)}");
        }
        finally
        {
            DisplayStatistics(settings.Host, stats);
        }

        return 0;
    }

    private static async Task PingOnceAsync(string host, int port, int sequence, PingStats stats)
    {
        try
        {
            // Note: We create a new connection for each ping because standard
            // servers often close the connection after the Status/Ping cycle.
            using var client = new ProtocolClient(host, port);
            
            await client.ConnectAsync();
            client.PerformHandshake();
            
            // We must request status first to be compliant with most server implementations
            client.RequestStatus(); 
            
            var latency = client.PerformPing();

            // Update Stats
            stats.Received++;
            stats.Sum += latency;
            if (latency < stats.Min) stats.Min = latency;
            if (latency > stats.Max) stats.Max = latency;

            var color = latency < 100 ? "green" : latency < 300 ? "yellow" : "red";
            AnsiConsole.MarkupLine($"Seq=[blue]{sequence}[/] Time=[{color}]{latency}ms[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"Seq=[blue]{sequence}[/] [red]Failed: {Markup.Escape(ex.Message)}[/]");
        }
    }

    private static void DisplayStatistics(string host, PingStats stats)
    {
        Console.WriteLine();
        AnsiConsole.Write(new Rule($"Ping statistics for {host}"));
        
        var loss = stats.Sent == 0 ? 0 : (double)(stats.Sent - stats.Received) / stats.Sent * 100;
        var avg = stats.Received == 0 ? 0 : stats.Sum / stats.Received;

        var grid = new Grid();
        grid.AddColumn();
        grid.AddColumn();
        
        grid.AddRow("Packets:", $"Sent = {stats.Sent}, Received = {stats.Received}, Lost = {stats.Sent - stats.Received} ({loss:F0}% loss)");
        
        if (stats.Received > 0)
        {
            grid.AddRow("Approximate round trip times:", $"Minimum = {stats.Min}ms, Maximum = {stats.Max}ms, Average = {avg}ms");
        }

        AnsiConsole.Write(grid);
        Console.WriteLine();
    }
}
