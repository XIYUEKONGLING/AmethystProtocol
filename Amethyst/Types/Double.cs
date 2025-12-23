using System.Buffers.Binary;
using Amethyst.Interfaces;

namespace Amethyst.Types;

public readonly struct Double(double value) : IType<Double>
{
    public double Value { get; } = value;

    public void Write(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[8];
        BinaryPrimitives.WriteDoubleBigEndian(buffer, Value);
        stream.Write(buffer);
    }

    public static Double Read(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[8];
        stream.ReadExactly(buffer);
        return new Double(BinaryPrimitives.ReadDoubleBigEndian(buffer));
    }

    public static implicit operator double(Double d) => d.Value;
    public static implicit operator Double(double d) => new(d);
}