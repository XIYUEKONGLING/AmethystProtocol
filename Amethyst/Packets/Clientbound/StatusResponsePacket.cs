using System.Text.Json;
using Amethyst.Core;
using Amethyst.Interfaces;
using Amethyst.Models;
using Amethyst.Types;

namespace Amethyst.Packets.Clientbound;

public class StatusResponsePacket(ServerStatus status) : IPacket
{
    public int Id => 0x00;
    public ServerStatus Status { get; } = status;

    public void Write(Stream stream)
    {
        var json = JsonSerializer.Serialize(Status);
        new TString(json).Write(stream);
    }

    public static StatusResponsePacket Read(Stream stream)
    {
        var jsonString = TString.Read(stream).Value;
        var status = JsonSerializer.Deserialize<ServerStatus>(jsonString);
        
        if (status is null)
            throw new InvalidDataException("Received null JSON status.");

        return new StatusResponsePacket(status);
    }
}