using Amethyst.Types;

namespace Amethyst.Tags.Primitives;

public class TagDouble(double value = 0) : Tag
{
    public double Value { get; set; } = value;
    public override TagType Type => TagType.Double;

    public override void WritePayload(Stream stream) => new TDouble(Value).Write(stream);
    public override void ReadPayload(Stream stream) => Value = TDouble.Read(stream);
}