using Amethyst.Types;

namespace AmethystTests.Types;

public class TDoubleTests
{
    [Theory]
    [InlineData(0.0d)]
    [InlineData(1.5d)]
    [InlineData(-123.456d)]
    [InlineData(double.MaxValue)]
    [InlineData(double.MinValue)]
    public void RoundTrip_ReturnsOriginalValue(double value)
    {
        // Arrange
        var original = new TDouble(value);
        using var stream = new MemoryStream();

        // Act
        original.Write(stream);
        stream.Position = 0;
        var result = TDouble.Read(stream);

        // Assert
        Assert.Equal(value, result.Value);
    }
}