using Amethyst.Interfaces;

namespace Amethyst.Types;

public readonly struct TVarInt(int value) : IType<TVarInt>
{
    private const int SegmentBits = 0x7F;
    private const int ContinueBit = 0x80;

    public int Value { get; } = value;

    public void Write(Stream stream)
    {
        var v = (uint)Value;

        while (true)
        {
            if ((v & ~SegmentBits) == 0)
            {
                stream.WriteByte((byte)v);
                return;
            }

            stream.WriteByte((byte)((v & SegmentBits) | ContinueBit));
            v >>= 7;
        }
    }

    public static TVarInt Read(Stream stream)
    {
        var value = 0;
        var position = 0;

        while (true)
        {
            var byteRead = stream.ReadByte();
            if (byteRead == -1)
            {
                throw new EndOfStreamException("End of stream reached while reading VarInt.");
            }

            var currentByte = (byte)byteRead;
            value |= (currentByte & SegmentBits) << position;

            if ((currentByte & ContinueBit) == 0) 
                break;

            position += 7;

            if (position >= 32)
            {
                throw new InvalidDataException("VarInt is too big.");
            }
        }

        return new TVarInt(value);
    }

    public static implicit operator int(TVarInt i) => i.Value;
    public static implicit operator TVarInt(int i) => new(i);
}