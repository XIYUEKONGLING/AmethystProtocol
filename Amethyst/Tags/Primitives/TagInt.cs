using Amethyst.Types;

namespace Amethyst.Tags.Primitives;

public class TagInt(int value = 0) : Tag
{
    public int Value { get; set; } = value;
    public override TagType Type => TagType.Int;

    public override void WritePayload(Stream stream) => new TInt(Value).Write(stream);
    public override void ReadPayload(Stream stream) => Value = TInt.Read(stream);
}