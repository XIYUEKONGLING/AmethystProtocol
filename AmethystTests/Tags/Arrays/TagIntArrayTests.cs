using Amethyst.Tags;
using Amethyst.Tags.Arrays;
using AmethystTests.Helpers;

namespace AmethystTests.Tags.Arrays;

public class TagIntArrayTests
{
    [Fact]
    public void WriteAndRead_ShouldPreserveValue()
    {
        int[] data = [1, 2, -100, int.MaxValue];
        var tag = new TagIntArray(data);
        var result = TagTestHelper.AssertRoundTrip(tag, () => new TagIntArray());
        
        Assert.Equal(data, result.Value);
    }

    [Fact]
    public void Type_ShouldBeIntArray()
    {
        Assert.Equal(TagType.IntArray, new TagIntArray().Type);
    }
}