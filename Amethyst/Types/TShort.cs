using System.Buffers.Binary;
using Amethyst.Interfaces;

namespace Amethyst.Types;

public readonly struct TShort(short value) : IType<TShort>
{
    public short Value { get; } = value;

    public void Write(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[2];
        BinaryPrimitives.WriteInt16BigEndian(buffer, Value);
        stream.Write(buffer);
    }

    public static TShort Read(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[2];
        stream.ReadExactly(buffer);
        return new TShort(BinaryPrimitives.ReadInt16BigEndian(buffer));
    }

    public static implicit operator short(TShort s) => s.Value;
    public static implicit operator TShort(short s) => new(s);
}