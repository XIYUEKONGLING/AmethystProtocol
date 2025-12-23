using Amethyst.Types;

namespace AmethystTests.Types;

public class TByteTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(127)]
    [InlineData(-128)]
    public void RoundTrip_ReturnsOriginalValue(sbyte value)
    {
        // Arrange
        var original = new TByte(value);
        using var stream = new MemoryStream();
        
        // Act
        original.Write(stream);
        stream.Position = 0;
        var result = TByte.Read(stream);

        // Assert
        Assert.Equal(value, result.Value);
    }
}