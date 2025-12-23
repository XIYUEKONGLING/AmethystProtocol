using Amethyst.Interfaces;

namespace Amethyst.Types;

public readonly struct UnsignedByte(byte value) : IType<UnsignedByte>
{
    public byte Value { get; } = value;

    public void Write(Stream stream)
    {
        stream.WriteByte(Value);
    }

    public static UnsignedByte Read(Stream stream)
    {
        var b = stream.ReadByte();
        if (b == -1)
        {
            throw new EndOfStreamException("End of stream reached while reading UnsignedByte.");
        }

        return new UnsignedByte((byte)b);
    }

    public static implicit operator byte(UnsignedByte b) => b.Value;
    public static implicit operator UnsignedByte(byte b) => new(b);
}