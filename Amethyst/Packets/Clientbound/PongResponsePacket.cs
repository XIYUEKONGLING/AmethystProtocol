using Amethyst.Interfaces;
using Amethyst.Types;

namespace Amethyst.Packets.Clientbound;

public class PongResponsePacket(long timestamp) : IPacket
{
    public int Id => 0x01;

    public long Timestamp { get; } = timestamp;

    public void Write(Stream stream)
    {
        new TLong(Timestamp).Write(stream);
    }

    public static PongResponsePacket Read(Stream stream)
    {
        var timestamp = TLong.Read(stream);
        return new PongResponsePacket(timestamp);
    }
}