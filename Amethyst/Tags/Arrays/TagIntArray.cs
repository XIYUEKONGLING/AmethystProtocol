using System.Buffers.Binary;
using Amethyst.Types;

namespace Amethyst.Tags.Arrays;

public class TagIntArray(int[]? value = null) : Tag
{
    public int[] Value { get; set; } = value ?? [];
    public override TagType Type => TagType.IntArray;

    public override void WritePayload(Stream stream)
    {
        new TInt(Value.Length).Write(stream);
        Span<byte> buffer = stackalloc byte[4];
        foreach (var i in Value)
        {
            BinaryPrimitives.WriteInt32BigEndian(buffer, i);
            stream.Write(buffer);
        }
    }

    public override void ReadPayload(Stream stream)
    {
        var len = TInt.Read(stream).Value;
        Value = new int[len];
        Span<byte> buffer = stackalloc byte[4];
        for (var i = 0; i < len; i++)
        {
            stream.ReadExactly(buffer);
            Value[i] = BinaryPrimitives.ReadInt32BigEndian(buffer);
        }
    }
}