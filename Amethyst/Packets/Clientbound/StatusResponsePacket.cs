using System.Text.Json;
using Amethyst.Core;
using Amethyst.Interfaces;
using Amethyst.Models;
using Amethyst.Types;

namespace Amethyst.Packets.Clientbound;

public class StatusResponsePacket(ServerStatus status) : IOutgoingPacket
{
    public int Id => 0x00;

    public void Write(Stream stream)
    {
        var json = JsonSerializer.Serialize(status);
        new TString(json).Write(stream);
    }
}