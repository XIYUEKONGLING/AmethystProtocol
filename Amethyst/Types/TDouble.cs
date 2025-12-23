using System.Buffers.Binary;
using Amethyst.Interfaces;

namespace Amethyst.Types;

public readonly struct TDouble(double value) : IType<TDouble>
{
    public double Value { get; } = value;

    public void Write(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[8];
        BinaryPrimitives.WriteDoubleBigEndian(buffer, Value);
        stream.Write(buffer);
    }

    public static TDouble Read(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[8];
        stream.ReadExactly(buffer);
        return new TDouble(BinaryPrimitives.ReadDoubleBigEndian(buffer));
    }

    public static implicit operator double(TDouble d) => d.Value;
    public static implicit operator TDouble(double d) => new(d);
}