using Amethyst.Types;

namespace Amethyst.Tags.Primitives;

public class TagLong(long value = 0) : Tag
{
    public long Value { get; set; } = value;
    public override TagType Type => TagType.Long;

    public override void WritePayload(Stream stream) => new TLong(Value).Write(stream);
    public override void ReadPayload(Stream stream) => Value = TLong.Read(stream);
}