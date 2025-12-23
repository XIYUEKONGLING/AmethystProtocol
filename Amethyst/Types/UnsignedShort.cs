using System.Buffers.Binary;
using Amethyst.Interfaces;

namespace Amethyst.Types;

public readonly struct UnsignedShort(ushort value) : IType<UnsignedShort>
{
    public ushort Value { get; } = value;

    public void Write(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[2];
        BinaryPrimitives.WriteUInt16BigEndian(buffer, Value);
        stream.Write(buffer);
    }

    public static UnsignedShort Read(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[2];
        stream.ReadExactly(buffer);
        return new UnsignedShort(BinaryPrimitives.ReadUInt16BigEndian(buffer));
    }

    public static implicit operator ushort(UnsignedShort s) => s.Value;
    public static implicit operator UnsignedShort(ushort s) => new(s);
}