using System.Buffers.Binary;
using Amethyst.Interfaces;

namespace Amethyst.Types;

public readonly struct Short(short value) : IType<Short>
{
    public short Value { get; } = value;

    public void Write(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[2];
        BinaryPrimitives.WriteInt16BigEndian(buffer, Value);
        stream.Write(buffer);
    }

    public static Short Read(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[2];
        stream.ReadExactly(buffer);
        return new Short(BinaryPrimitives.ReadInt16BigEndian(buffer));
    }

    public static implicit operator short(Short s) => s.Value;
    public static implicit operator Short(short s) => new(s);
}