using Amethyst.Types;

namespace AmethystTests.Types;

public class TIntTests
{
    [Fact]
    public void Write_IsBigEndian()
    {
        // Arrange
        int value = 1; // 0x00000001
        var tInt = new TInt(value);
        using var stream = new MemoryStream();

        // Act
        tInt.Write(stream);

        // Assert
        var bytes = stream.ToArray();
        Assert.Equal(0x00, bytes[0]);
        Assert.Equal(0x01, bytes[3]);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void RoundTrip_ReturnsOriginalValue(int value)
    {
        // Arrange
        var original = new TInt(value);
        using var stream = new MemoryStream();

        // Act
        original.Write(stream);
        stream.Position = 0;
        var result = TInt.Read(stream);

        // Assert
        Assert.Equal(value, result.Value);
    }
}