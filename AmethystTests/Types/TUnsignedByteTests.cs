using Amethyst.Types;

namespace AmethystTests.Types;

public class TUnsignedByteTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(255)]
    public void RoundTrip_ReturnsOriginalValue(byte value)
    {
        // Arrange
        var original = new TUnsignedByte(value);
        using var stream = new MemoryStream();

        // Act
        original.Write(stream);
        stream.Position = 0;
        var result = TUnsignedByte.Read(stream);

        // Assert
        Assert.Equal(value, result.Value);
    }
}