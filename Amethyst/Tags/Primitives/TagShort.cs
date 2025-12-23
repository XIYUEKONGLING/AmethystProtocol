using Amethyst.Types;

namespace Amethyst.Tags.Primitives;

public class TagShort(short value = 0) : Tag
{
    public short Value { get; set; } = value;
    public override TagType Type => TagType.Short;

    public override void WritePayload(Stream stream) => new TShort(Value).Write(stream);
    public override void ReadPayload(Stream stream) => Value = TShort.Read(stream);
}