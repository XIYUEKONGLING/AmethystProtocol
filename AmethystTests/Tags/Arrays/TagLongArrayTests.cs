using Amethyst.Tags;
using Amethyst.Tags.Arrays;
using AmethystTests.Helpers;

namespace AmethystTests.Tags.Arrays;

public class TagLongArrayTests
{
    [Fact]
    public void WriteAndRead_ShouldPreserveValue()
    {
        long[] data = [100L, -500L, long.MaxValue];
        var tag = new TagLongArray(data);
        var result = TagTestHelper.AssertRoundTrip(tag, () => new TagLongArray());
        
        Assert.Equal(data, result.Value);
    }

    [Fact]
    public void Type_ShouldBeLongArray()
    {
        Assert.Equal(TagType.LongArray, new TagLongArray().Type);
    }
}