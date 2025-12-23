using System.Buffers.Binary;
using Amethyst.Interfaces;

namespace Amethyst.Types;

public readonly struct TUnsignedShort(ushort value) : IType<TUnsignedShort>
{
    public ushort Value { get; } = value;

    public void Write(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[2];
        BinaryPrimitives.WriteUInt16BigEndian(buffer, Value);
        stream.Write(buffer);
    }

    public static TUnsignedShort Read(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[2];
        stream.ReadExactly(buffer);
        return new TUnsignedShort(BinaryPrimitives.ReadUInt16BigEndian(buffer));
    }

    public static implicit operator ushort(TUnsignedShort s) => s.Value;
    public static implicit operator TUnsignedShort(ushort s) => new(s);
}