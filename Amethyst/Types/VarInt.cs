using Amethyst.Interfaces;

namespace Amethyst.Types;

public readonly struct VarInt(int value) : IType<VarInt>
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

    public static VarInt Read(Stream stream)
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

        return new VarInt(value);
    }

    public static implicit operator int(VarInt i) => i.Value;
    public static implicit operator VarInt(int i) => new(i);
}