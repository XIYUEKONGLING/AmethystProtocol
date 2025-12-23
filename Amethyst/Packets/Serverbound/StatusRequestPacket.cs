using Amethyst.Core;
using Amethyst.Interfaces;

namespace Amethyst.Packets.Serverbound;

public class StatusRequestPacket : IPacket
{
    public int Id => 0x00;

    public void Write(Stream stream)
    {
        // Empty body
    }

    public static StatusRequestPacket Read(Stream stream)
    {
        return new StatusRequestPacket();
    }
}