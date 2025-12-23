using Amethyst.Interfaces;

namespace Amethyst.Types;

public readonly struct Boolean(bool value) : IType<Boolean>
{
    public bool Value { get; } = value;

    public void Write(Stream stream)
    {
        stream.WriteByte(Value ? (byte)0x01 : (byte)0x00);
    }

    public static Boolean Read(Stream stream)
    {
        var b = stream.ReadByte();
        if (b == -1)
        {
            throw new EndOfStreamException("End of stream reached while reading Boolean.");
        }

        return new Boolean(b == 0x01);
    }

    public static implicit operator bool(Boolean b) => b.Value;
    public static implicit operator Boolean(bool b) => new(b);
}