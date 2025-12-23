using System.Buffers.Binary;
using Amethyst.Types;

namespace Amethyst.Tags.Arrays;

public class TagLongArray(long[]? value = null) : Tag
{
    public long[] Value { get; set; } = value ?? [];
    public override TagType Type => TagType.LongArray;

    public override void WritePayload(Stream stream)
    {
        new TInt(Value.Length).Write(stream);
        Span<byte> buffer = stackalloc byte[8];
        foreach (var l in Value)
        {
            BinaryPrimitives.WriteInt64BigEndian(buffer, l);
            stream.Write(buffer);
        }
    }

    public override void ReadPayload(Stream stream)
    {
        var len = TInt.Read(stream).Value;
        Value = new long[len];
        Span<byte> buffer = stackalloc byte[8];
        for (var i = 0; i < len; i++)
        {
            stream.ReadExactly(buffer);
            Value[i] = BinaryPrimitives.ReadInt64BigEndian(buffer);
        }
    }
}