using System.Buffers.Binary;
using Amethyst.Interfaces;

namespace Amethyst.Types;

public readonly struct TInt(int value) : IType<TInt>
{
    public int Value { get; } = value;

    public void Write(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[4];
        BinaryPrimitives.WriteInt32BigEndian(buffer, Value);
        stream.Write(buffer);
    }

    public static TInt Read(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[4];
        stream.ReadExactly(buffer);
        return new TInt(BinaryPrimitives.ReadInt32BigEndian(buffer));
    }

    public static implicit operator int(TInt i) => i.Value;
    public static implicit operator TInt(int i) => new(i);
}