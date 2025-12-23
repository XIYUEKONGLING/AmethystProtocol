using Amethyst.Types;

namespace AmethystTests.Types;

public class TBooleanTests
{
    [Theory]
    [InlineData(true, 0x01)]
    [InlineData(false, 0x00)]
    public void Write_EncodesCorrectly(bool value, byte expectedByte)
    {
        // Arrange
        var boolean = new TBoolean(value);
        using var stream = new MemoryStream();

        // Act
        boolean.Write(stream);

        // Assert
        Assert.Equal(expectedByte, stream.ToArray()[0]);
    }

    [Fact]
    public void Read_RoundTrip_ReturnsOriginalValue()
    {
        // Arrange
        var original = new TBoolean(true);
        using var stream = new MemoryStream();
        original.Write(stream);
        stream.Position = 0;

        // Act
        var result = TBoolean.Read(stream);

        // Assert
        Assert.True(result.Value);
    }
}