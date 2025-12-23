using Amethyst.Interfaces;

namespace Amethyst.Types;

public readonly struct Byte(sbyte value) : IType<Byte>
{
    public sbyte Value { get; } = value;

    public void Write(Stream stream)
    {
        stream.WriteByte((byte)Value);
    }

    public static Byte Read(Stream stream)
    {
        var b = stream.ReadByte();
        if (b == -1)
        {
            throw new EndOfStreamException("End of stream reached while reading Byte.");
        }

        return new Byte((sbyte)b);
    }

    public static implicit operator sbyte(Byte b) => b.Value;
    public static implicit operator Byte(sbyte b) => new(b);
}