using Amethyst.Interfaces;

namespace Amethyst.Types;

public readonly struct TByte(sbyte value) : IType<TByte>
{
    public sbyte Value { get; } = value;

    public void Write(Stream stream)
    {
        stream.WriteByte((byte)Value);
    }

    public static TByte Read(Stream stream)
    {
        var b = stream.ReadByte();
        if (b == -1)
        {
            throw new EndOfStreamException("End of stream reached while reading Byte.");
        }

        return new TByte((sbyte)b);
    }

    public static implicit operator sbyte(TByte b) => b.Value;
    public static implicit operator TByte(sbyte b) => new(b);
}