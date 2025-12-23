using Amethyst.Tags;
using Amethyst.Tags.Collections;
using Amethyst.Tags.Primitives;
using AmethystTests.Helpers;

namespace AmethystTests.Tags.Collections;

public class TagListTests
{
    [Fact]
    public void WriteAndRead_ShouldPreserveElements()
    {
        var list = new TagList
        {
            new TagInt(1),
            new TagInt(2)
        };
        
        var result = TagTestHelper.AssertRoundTrip(list, () => new TagList());
        
        Assert.Equal(2, result.Count);
        Assert.Equal(TagType.Int, result.ListType);
    }

    [Fact]
    public void Write_MixedTypes_ShouldThrow()
    {
        var list = new TagList
        {
            new TagInt(1),
            new TagString("Bad") // Mixed types
        };

        using var stream = new MemoryStream();
        Assert.Throws<InvalidDataException>(() => list.WritePayload(stream));
    }

    [Fact]
    public void Type_ShouldBeList()
    {
        Assert.Equal(TagType.List, new TagList().Type);
    }
}