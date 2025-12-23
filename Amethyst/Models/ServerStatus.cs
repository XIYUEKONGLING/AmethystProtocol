using System.Text.Json.Serialization;

namespace Amethyst.Models;

public class ServerStatus
{
    [JsonPropertyName("version")]
    public required VersionPayload Version { get; init; }

    [JsonPropertyName("players")]
    public required PlayersPayload Players { get; init; }

    [JsonPropertyName("description")]
    public required DescriptionPayload Description { get; init; }

    [JsonPropertyName("favicon")]
    public string? Favicon { get; init; } // "data:image/png;base64,<data>"

    [JsonPropertyName("enforcesSecureChat")]
    public bool EnforcesSecureChat { get; init; }
}

public class VersionPayload
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("protocol")]
    public required int Protocol { get; init; }
}

public class PlayersPayload
{
    [JsonPropertyName("max")]
    public required int Max { get; init; }

    [JsonPropertyName("online")]
    public required int Online { get; init; }

    [JsonPropertyName("sample")]
    public List<PlayerSample> Sample { get; init; } = [];
}

public class PlayerSample
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("id")]
    public required string Id { get; init; } // UUID
}

public class DescriptionPayload
{
    [JsonPropertyName("text")]
    public required string Text { get; init; }
}