using Amethyst.Tags;
using Amethyst.Tags.Primitives;

namespace AmethystTests.Tags.Primitives;

public class TagEndTests
{
    [Fact]
    public void WriteAndRead_ShouldDoNothing()
    {
        // TagEnd has no payload, so we just check it doesn't throw
        var tag = new TagEnd();
        using var stream = new MemoryStream();
        tag.WritePayload(stream);
        
        Assert.Equal(0, stream.Length);

        tag.ReadPayload(stream); // Should not throw
    }

    [Fact]
    public void Type_ShouldBeEnd()
    {
        Assert.Equal(TagType.End, new TagEnd().Type);
    }
}