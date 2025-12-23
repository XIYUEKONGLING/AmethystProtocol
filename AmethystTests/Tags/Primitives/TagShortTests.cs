using Amethyst.Tags;
using Amethyst.Tags.Primitives;
using AmethystTests.Helpers;

namespace AmethystTests.Tags.Primitives;

public class TagShortTests
{
    [Fact]
    public void WriteAndRead_ShouldPreserveValue()
    {
        var tag = new TagShort(12345);
        var result = TagTestHelper.AssertRoundTrip(tag, () => new TagShort());
        Assert.Equal(12345, result.Value);
    }

    [Fact]
    public void Type_ShouldBeShort()
    {
        Assert.Equal(TagType.Short, new TagShort().Type);
    }
}