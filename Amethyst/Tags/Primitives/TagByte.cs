using Amethyst.Types;

namespace Amethyst.Tags.Primitives;

public class TagByte(sbyte value = 0) : Tag
{
    public sbyte Value { get; set; } = value;
    public override TagType Type => TagType.Byte;

    public override void WritePayload(Stream stream) => new TByte(Value).Write(stream);
    public override void ReadPayload(Stream stream) => Value = TByte.Read(stream);
}