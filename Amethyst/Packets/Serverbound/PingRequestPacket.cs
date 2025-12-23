using Amethyst.Interfaces;
using Amethyst.Types;

namespace Amethyst.Packets.Serverbound;

public class PingRequestPacket(long timestamp) : IPacket
{
    public int Id => 0x01;

    public long Timestamp { get; } = timestamp;

    public void Write(Stream stream)
    {
        new TLong(Timestamp).Write(stream);
    }

    public static PingRequestPacket Read(Stream stream)
    {
        var timestamp = TLong.Read(stream);
        return new PingRequestPacket(timestamp);
    }
}