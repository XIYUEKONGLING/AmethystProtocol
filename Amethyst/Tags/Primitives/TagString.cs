using System.Text;
using Amethyst.Types;

namespace Amethyst.Tags.Primitives;

public class TagString(string value = "") : Tag
{
    public string Value { get; set; } = value;
    public override TagType Type => TagType.String;

    public override void WritePayload(Stream stream)
    {
        var bytes = Encoding.UTF8.GetBytes(Value);
        new TUnsignedShort((ushort)bytes.Length).Write(stream);
        stream.Write(bytes);
    }

    public override void ReadPayload(Stream stream)
    {
        var len = TUnsignedShort.Read(stream).Value;
        Span<byte> buffer = len <= 1024 ? stackalloc byte[len] : new byte[len];
        stream.ReadExactly(buffer);
        Value = Encoding.UTF8.GetString(buffer);
    }
}