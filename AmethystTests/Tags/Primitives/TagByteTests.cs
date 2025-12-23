using Amethyst.Tags;
using Amethyst.Tags.Primitives;
using AmethystTests.Helpers;

namespace AmethystTests.Tags.Primitives;

public class TagByteTests
{
    [Fact]
    public void WriteAndRead_ShouldPreserveValue()
    {
        var tag = new TagByte(123);
        var result = TagTestHelper.AssertRoundTrip(tag, () => new TagByte());
        Assert.Equal(123, result.Value);
    }

    [Fact]
    public void Type_ShouldBeByte()
    {
        Assert.Equal(TagType.Byte, new TagByte().Type);
    }
}