using Amethyst.Tags;
using Amethyst.Tags.Primitives;
using AmethystTests.Helpers;

namespace AmethystTests.Tags.Primitives;

public class TagLongTests
{
    [Fact]
    public void WriteAndRead_ShouldPreserveValue()
    {
        var tag = new TagLong(long.MaxValue);
        var result = TagTestHelper.AssertRoundTrip(tag, () => new TagLong());
        Assert.Equal(long.MaxValue, result.Value);
    }

    [Fact]
    public void Type_ShouldBeLong()
    {
        Assert.Equal(TagType.Long, new TagLong().Type);
    }
}