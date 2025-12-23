using System.Buffers.Binary;
using Amethyst.Interfaces;

namespace Amethyst.Types;

public readonly struct Int(int value) : IType<Int>
{
    public int Value { get; } = value;

    public void Write(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[4];
        BinaryPrimitives.WriteInt32BigEndian(buffer, Value);
        stream.Write(buffer);
    }

    public static Int Read(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[4];
        stream.ReadExactly(buffer);
        return new Int(BinaryPrimitives.ReadInt32BigEndian(buffer));
    }

    public static implicit operator int(Int i) => i.Value;
    public static implicit operator Int(int i) => new(i);
}