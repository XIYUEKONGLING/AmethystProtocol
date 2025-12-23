using Amethyst.Types;

namespace AmethystTests.Types;

public class TIdentifierTests
{
    [Fact]
    public void Constructor_WithNamespaceAndValue_SetsProperties()
    {
        // Arrange
        var ns = "custom";
        var val = "item";

        // Act
        var id = new TIdentifier(ns, val);

        // Assert
        Assert.Equal(ns, id.Namespace);
        Assert.Equal(val, id.Value);
        Assert.Equal("custom:item", id.ToString());
    }

    [Fact]
    public void Constructor_SingleString_DefaultsToMinecraftNamespace()
    {
        // Arrange
        var input = "dirt";

        // Act
        var id = new TIdentifier(input);

        // Assert
        Assert.Equal("minecraft", id.Namespace);
        Assert.Equal("dirt", id.Value);
        Assert.Equal("minecraft:dirt", id.ToString());
    }

    [Fact]
    public void Constructor_FullString_ParsesCorrectly()
    {
        // Arrange
        var input = "minecraft:stone";

        // Act
        var id = new TIdentifier(input);

        // Assert
        Assert.Equal("minecraft", id.Namespace);
        Assert.Equal("stone", id.Value);
    }

    [Theory]
    [InlineData("Invalid Namespace:thing")] // Space in namespace
    [InlineData("minecraft:InvalidValue")] // Uppercase in value
    [InlineData("minecraft:value!")] // Special char in value
    public void Constructor_InvalidFormat_ThrowsArgumentException(string invalidId)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new TIdentifier(invalidId));
    }

    [Fact]
    public void Read_RoundTrip_ReturnsOriginalIdentifier()
    {
        // Arrange
        var original = new TIdentifier("custom:test_block");
        using var stream = new MemoryStream();
        original.Write(stream);
        stream.Position = 0;

        // Act
        var result = TIdentifier.Read(stream);

        // Assert
        Assert.Equal(original.ToString(), result.ToString());
    }
}