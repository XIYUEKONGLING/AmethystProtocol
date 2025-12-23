using Amethyst.Types;

namespace AmethystTests.Types;

public class TFloatTests
{
    [Theory]
    [InlineData(0.0f)]
    [InlineData(1.5f)]
    [InlineData(-123.456f)]
    [InlineData(float.MaxValue)]
    [InlineData(float.MinValue)]
    public void RoundTrip_ReturnsOriginalValue(float value)
    {
        // Arrange
        var original = new TFloat(value);
        using var stream = new MemoryStream();

        // Act
        original.Write(stream);
        stream.Position = 0;
        var result = TFloat.Read(stream);

        // Assert
        Assert.Equal(value, result.Value);
    }
}