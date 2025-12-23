using Amethyst.Tags;
using Amethyst.Tags.Arrays;
using AmethystTests.Helpers;

namespace AmethystTests.Tags.Arrays;

public class TagByteArrayTests
{
    [Fact]
    public void WriteAndRead_ShouldPreserveValue()
    {
        byte[] data = [0x01, 0x02, 0xFF];
        var tag = new TagByteArray(data);
        var result = TagTestHelper.AssertRoundTrip(tag, () => new TagByteArray());
        
        Assert.Equal(data, result.Value);
    }

    [Fact]
    public void Type_ShouldBeByteArray()
    {
        Assert.Equal(TagType.ByteArray, new TagByteArray().Type);
    }
}