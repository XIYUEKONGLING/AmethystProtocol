using Amethyst.Interfaces;

namespace Amethyst.Packets.Serverbound;

public class StatusRequestPacket : IIncomingPacket<StatusRequestPacket>
{
    public static int Id => 0x00;

    public static StatusRequestPacket Read(Stream stream)
    {
        // No fields to read
        return new StatusRequestPacket();
    }
}