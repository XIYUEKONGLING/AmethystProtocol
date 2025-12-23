using Amethyst.Interfaces;

namespace Amethyst.Types;

public readonly struct TVarLong(long value) : IType<TVarLong>
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

    public static TVarLong Read(Stream stream)
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

        return new TVarLong(value);
    }

    public static implicit operator long(TVarLong l) => l.Value;
    public static implicit operator TVarLong(long l) => new(l);
}