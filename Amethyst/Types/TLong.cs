using System.Buffers.Binary;
using Amethyst.Interfaces;

namespace Amethyst.Types;

public readonly struct TLong(long value) : IType<TLong>
{
    public long Value { get; } = value;

    public void Write(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[8];
        BinaryPrimitives.WriteInt64BigEndian(buffer, Value);
        stream.Write(buffer);
    }

    public static TLong Read(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[8];
        stream.ReadExactly(buffer);
        return new TLong(BinaryPrimitives.ReadInt64BigEndian(buffer));
    }

    public static implicit operator long(TLong l) => l.Value;
    public static implicit operator TLong(long l) => new(l);
}