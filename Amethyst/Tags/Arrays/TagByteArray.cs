using Amethyst.Types;

namespace Amethyst.Tags.Arrays;

public class TagByteArray(byte[]? value = null) : Tag
{
    public byte[] Value { get; set; } = value ?? [];
    public override TagType Type => TagType.ByteArray;

    public override void WritePayload(Stream stream)
    {
        new TInt(Value.Length).Write(stream);
        stream.Write(Value);
    }

    public override void ReadPayload(Stream stream)
    {
        var len = TInt.Read(stream).Value;
        Value = new byte[len];
        stream.ReadExactly(Value);
    }
}