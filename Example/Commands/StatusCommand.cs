using System.Text;
using System.Text.Json;
using Example.Network;
using Example.Settings;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Example.Commands;

public class StatusCommand : AsyncCommand<ServerSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, ServerSettings settings, CancellationToken cancellationToken)
    {
        try
        {
            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .StartAsync($"Connecting to {settings.Host}:{settings.Port}...", async ctx =>
                {
                    using var client = new ProtocolClient(settings.Host, settings.Port);
                    
                    await client.ConnectAsync();
                    ctx.Status("Performing Handshake...");
                    client.PerformHandshake();

                    ctx.Status("Fetching Status...");
                    var status = client.RequestStatus();

                    ctx.Status("Pinging...");
                    var latency = client.PerformPing();

                    DisplayStatus(status, latency);
                });

            return 0;
        }
        catch (Exception ex)
        {
            // Fix: Escape the exception message to prevent markup parsing errors
            AnsiConsole.MarkupLine($"[red]Error:[/] {Markup.Escape(ex.Message)}");
            return 1;
        }
    }

    private static void DisplayStatus(Amethyst.Models.ServerStatus status, long latency)
    {
        var table = new Table();
        table.Border(TableBorder.Rounded);
        table.AddColumn("Property");
        table.AddColumn("Value");

        // Version
        table.AddRow("Version", $"[blue]{status.Version?.Name ?? "Unknown"}[/] (Protocol: {status.Version?.Protocol})");

        // Description (MOTD)
        var description = ExtractText(status.Description);
        if (string.IsNullOrWhiteSpace(description)) description = "No description provided.";
        
        // Remove legacy formatting codes (e.g. §a, §1) for clean display
        description = System.Text.RegularExpressions.Regex.Replace(description, "§[0-9a-fk-or]", "");
        table.AddRow("Description", $"[italic]{Markup.Escape(description)}[/]");

        // Players
        var online = status.Players?.Online ?? 0;
        var max = status.Players?.Max ?? 0;
        table.AddRow("Players", $"[green]{online}[/] / [grey]{max}[/]");

        // Secure Chat
        var secureChat = status.EnforcesSecureChat ? "[green]Yes[/]" : "[red]No[/]";
        table.AddRow("Secure Chat", secureChat);

        // Favicon (Size only)
        var faviconSize = string.IsNullOrEmpty(status.Favicon) 
            ? "[grey]None[/]" 
            : $"[yellow]{status.Favicon.Length} bytes[/] (Base64)";
        table.AddRow("Favicon", faviconSize);

        // Latency
        var latencyColor = latency < 100 ? "green" : latency < 300 ? "yellow" : "red";
        table.AddRow("Latency", $"[{latencyColor}]{latency} ms[/]");

        AnsiConsole.Write(table);
    }

    /// <summary>
    /// Recursively extracts text from a complex JSON chat component.
    /// </summary>
    private static string ExtractText(JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.String)
        {
            return element.GetString() ?? string.Empty;
        }

        if (element.ValueKind == JsonValueKind.Array)
        {
            var sb = new StringBuilder();
            foreach (var item in element.EnumerateArray())
            {
                sb.Append(ExtractText(item));
            }
            return sb.ToString();
        }

        if (element.ValueKind == JsonValueKind.Object)
        {
            var sb = new StringBuilder();

            // 1. "text" property
            if (element.TryGetProperty("text", out var textProp))
            {
                sb.Append(textProp.ValueKind == JsonValueKind.String ? textProp.GetString() : ExtractText(textProp));
            }

            // 2. "extra" property (list of siblings)
            if (element.TryGetProperty("extra", out var extraProp) && extraProp.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in extraProp.EnumerateArray())
                {
                    sb.Append(ExtractText(item));
                }
            }

            // Note: "translate" components are ignored here as we don't have a translation key map.
            return sb.ToString();
        }

        return string.Empty;
    }
}
