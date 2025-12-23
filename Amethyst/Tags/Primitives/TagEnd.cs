namespace Amethyst.Tags.Primitives;

public class TagEnd : Tag
{
    public override TagType Type => TagType.End;
    public override void WritePayload(Stream stream) { } // No payload
    public override void ReadPayload(Stream stream) { }
}