using System.ComponentModel;
using Spectre.Console.Cli;

namespace Example.Settings;

public class PingSettings : ServerSettings
{
    [CommandOption("-c|--count")]
    [Description("Number of pings to send (default: infinite).")]
    public int? Count { get; init; }

    [CommandOption("-i|--interval")]
    [Description("Interval between pings in milliseconds (default: 1000).")]
    [DefaultValue(1000)]
    public int Interval { get; init; } = 1000;
}