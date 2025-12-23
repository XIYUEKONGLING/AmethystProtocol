using System.Buffers.Binary;
using Amethyst.Interfaces;

namespace Amethyst.Types;

public readonly struct TFloat(float value) : IType<TFloat>
{
    public float Value { get; } = value;

    public void Write(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[4];
        BinaryPrimitives.WriteSingleBigEndian(buffer, Value);
        stream.Write(buffer);
    }

    public static TFloat Read(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[4];
        stream.ReadExactly(buffer);
        return new TFloat(BinaryPrimitives.ReadSingleBigEndian(buffer));
    }

    public static implicit operator float(TFloat f) => f.Value;
    public static implicit operator TFloat(float f) => new(f);
}