using Amethyst.Tags;
using Amethyst.Tags.Collections;
using Amethyst.Tags.Primitives;
using AmethystTests.Helpers;

namespace AmethystTests.Tags.Collections;

public class TagCompoundTests
{
    [Fact]
    public void WriteAndRead_ShouldPreserveStructure()
    {
        var compound = new TagCompound
        {
            { "myInt", new TagInt(42) },
            { "myString", new TagString("Test") }
        };

        var result = TagTestHelper.AssertRoundTrip(compound, () => new TagCompound());

        Assert.True(result.ContainsKey("myInt"));
        Assert.True(result.ContainsKey("myString"));
        Assert.Equal(42, result.GetInt("myInt"));
        Assert.Equal("Test", result.GetString("myString"));
    }

    [Fact]
    public void GetHelpers_ShouldReturnDefaults_WhenMissing()
    {
        var compound = new TagCompound();
        Assert.Equal(0, compound.GetInt("missing"));
        Assert.Equal(string.Empty, compound.GetString("missing"));
    }

    [Fact]
    public void Type_ShouldBeCompound()
    {
        Assert.Equal(TagType.Compound, new TagCompound().Type);
    }
}