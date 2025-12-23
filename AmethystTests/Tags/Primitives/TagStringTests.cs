using Amethyst.Tags;
using Amethyst.Tags.Primitives;
using AmethystTests.Helpers;

namespace AmethystTests.Tags.Primitives;

public class TagStringTests
{
    [Theory]
    [InlineData("Hello World")]
    [InlineData("")]
    [InlineData("你好世界")] // UTF-8 check
    public void WriteAndRead_ShouldPreserveValue(string value)
    {
        var tag = new TagString(value);
        var result = TagTestHelper.AssertRoundTrip(tag, () => new TagString());
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void Type_ShouldBeString()
    {
        Assert.Equal(TagType.String, new TagString().Type);
    }
}