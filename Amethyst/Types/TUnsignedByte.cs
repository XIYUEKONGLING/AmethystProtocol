using Amethyst.Interfaces;

namespace Amethyst.Types;

public readonly struct TUnsignedByte(byte value) : IType<TUnsignedByte>
{
    public byte Value { get; } = value;

    public void Write(Stream stream)
    {
        stream.WriteByte(Value);
    }

    public static TUnsignedByte Read(Stream stream)
    {
        var b = stream.ReadByte();
        if (b == -1)
        {
            throw new EndOfStreamException("End of stream reached while reading UnsignedByte.");
        }

        return new TUnsignedByte((byte)b);
    }

    public static implicit operator byte(TUnsignedByte b) => b.Value;
    public static implicit operator TUnsignedByte(byte b) => new(b);
}