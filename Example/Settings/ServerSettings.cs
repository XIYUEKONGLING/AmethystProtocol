using System.ComponentModel;
using Spectre.Console.Cli;

namespace Example.Settings;

public class ServerSettings : CommandSettings
{
    [CommandArgument(0, "[HOST]")]
    [Description("The server hostname (default: localhost).")]
    [DefaultValue("localhost")]
    public string Host { get; init; } = "localhost";

    [CommandArgument(1, "[PORT]")]
    [Description("The server port (default: 25565).")]
    [DefaultValue(25565)]
    public int Port { get; init; } = 25565;
}