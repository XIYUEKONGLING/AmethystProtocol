using Amethyst.Types;

namespace Amethyst.Tags.Primitives;

public class TagFloat(float value = 0) : Tag
{
    public float Value { get; set; } = value;
    public override TagType Type => TagType.Float;

    public override void WritePayload(Stream stream) => new TFloat(Value).Write(stream);
    public override void ReadPayload(Stream stream) => Value = TFloat.Read(stream);
}