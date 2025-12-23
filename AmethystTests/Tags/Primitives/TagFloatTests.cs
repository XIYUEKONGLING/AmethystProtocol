using Amethyst.Tags;
using Amethyst.Tags.Primitives;
using AmethystTests.Helpers;

namespace AmethystTests.Tags.Primitives;

public class TagFloatTests
{
    [Fact]
    public void WriteAndRead_ShouldPreserveValue()
    {
        var tag = new TagFloat(123.456f);
        var result = TagTestHelper.AssertRoundTrip(tag, () => new TagFloat());
        Assert.Equal(123.456f, result.Value);
    }

    [Fact]
    public void Type_ShouldBeFloat()
    {
        Assert.Equal(TagType.Float, new TagFloat().Type);
    }
}