using System.Buffers.Binary;
using Amethyst.Interfaces;

namespace Amethyst.Types;

public readonly struct Float(float value) : IType<Float>
{
    public float Value { get; } = value;

    public void Write(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[4];
        BinaryPrimitives.WriteSingleBigEndian(buffer, Value);
        stream.Write(buffer);
    }

    public static Float Read(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[4];
        stream.ReadExactly(buffer);
        return new Float(BinaryPrimitives.ReadSingleBigEndian(buffer));
    }

    public static implicit operator float(Float f) => f.Value;
    public static implicit operator Float(float f) => new(f);
}