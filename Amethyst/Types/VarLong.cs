using Amethyst.Interfaces;

namespace Amethyst.Types;

public readonly struct VarLong(long value) : IType<VarLong>
{
    private const int SegmentBits = 0x7F;
    private const int ContinueBit = 0x80;

    public long Value { get; } = value;

    public void Write(Stream stream)
    {
        var v = (ulong)Value;

        while (true)
        {
            if ((v & ~((ulong)SegmentBits)) == 0)
            {
                stream.WriteByte((byte)v);
                return;
            }

            stream.WriteByte((byte)((v & SegmentBits) | ContinueBit));
            v >>= 7;
        }
    }

    public static VarLong Read(Stream stream)
    {
        long value = 0;
        var position = 0;

        while (true)
        {
            var byteRead = stream.ReadByte();
            if (byteRead == -1)
            {
                throw new EndOfStreamException("End of stream reached while reading VarLong.");
            }

            var currentByte = (byte)byteRead;
            value |= (long)(currentByte & SegmentBits) << position;

            if ((currentByte & ContinueBit) == 0) 
                break;

            position += 7;

            if (position >= 64)
            {
                throw new InvalidDataException("VarLong is too big.");
            }
        }

        return new VarLong(value);
    }

    public static implicit operator long(VarLong l) => l.Value;
    public static implicit operator VarLong(long l) => new(l);
}