using Amethyst.Tags;
using Amethyst.Tags.Primitives;
using AmethystTests.Helpers;

namespace AmethystTests.Tags.Primitives;

public class TagIntTests
{
    [Fact]
    public void WriteAndRead_ShouldPreserveValue()
    {
        var tag = new TagInt(int.MaxValue);
        var result = TagTestHelper.AssertRoundTrip(tag, () => new TagInt());
        Assert.Equal(int.MaxValue, result.Value);
    }

    [Fact]
    public void Type_ShouldBeInt()
    {
        Assert.Equal(TagType.Int, new TagInt().Type);
    }
}