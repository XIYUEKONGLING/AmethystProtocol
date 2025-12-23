using Amethyst.Types;

namespace AmethystTests.Types;

public class TLongTests
{
    [Fact]
    public void Write_IsBigEndian()
    {
        // Arrange
        long value = 1; 
        var tLong = new TLong(value);
        using var stream = new MemoryStream();

        // Act
        tLong.Write(stream);

        // Assert
        var bytes = stream.ToArray();
        Assert.Equal(8, bytes.Length);
        Assert.Equal(0x00, bytes[0]);
        Assert.Equal(0x01, bytes[7]);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(long.MaxValue)]
    [InlineData(long.MinValue)]
    public void RoundTrip_ReturnsOriginalValue(long value)
    {
        // Arrange
        var original = new TLong(value);
        using var stream = new MemoryStream();

        // Act
        original.Write(stream);
        stream.Position = 0;
        var result = TLong.Read(stream);

        // Assert
        Assert.Equal(value, result.Value);
    }
}