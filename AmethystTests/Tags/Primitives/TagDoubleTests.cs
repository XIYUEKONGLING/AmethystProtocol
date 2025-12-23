using Amethyst.Tags;
using Amethyst.Tags.Primitives;
using AmethystTests.Helpers;

namespace AmethystTests.Tags.Primitives;

public class TagDoubleTests
{
    [Fact]
    public void WriteAndRead_ShouldPreserveValue()
    {
        var tag = new TagDouble(123.45678910);
        var result = TagTestHelper.AssertRoundTrip(tag, () => new TagDouble());
        Assert.Equal(123.45678910, result.Value);
    }

    [Fact]
    public void Type_ShouldBeDouble()
    {
        Assert.Equal(TagType.Double, new TagDouble().Type);
    }
}