using System.Buffers.Binary;
using Amethyst.Interfaces;

namespace Amethyst.Types;

public readonly struct Long(long value) : IType<Long>
{
    public long Value { get; } = value;

    public void Write(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[8];
        BinaryPrimitives.WriteInt64BigEndian(buffer, Value);
        stream.Write(buffer);
    }

    public static Long Read(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[8];
        stream.ReadExactly(buffer);
        return new Long(BinaryPrimitives.ReadInt64BigEndian(buffer));
    }

    public static implicit operator long(Long l) => l.Value;
    public static implicit operator Long(long l) => new(l);
}