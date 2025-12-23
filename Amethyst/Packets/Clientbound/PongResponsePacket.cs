using Amethyst.Interfaces;
using Amethyst.Types;

namespace Amethyst.Packets.Clientbound;

public class PongResponsePacket(long timestamp) : IOutgoingPacket
{
    public int Id => 0x01;

    public void Write(Stream stream)
    {
        new TLong(timestamp).Write(stream);
    }
}