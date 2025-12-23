using System.Buffers.Binary;
using Amethyst.Types;

namespace AmethystTests.Types;

public class TShortTests
{
    [Fact]
    public void Write_IsBigEndian()
    {
        // Arrange
        short value = 1; // 0x0001
        var tShort = new TShort(value);
        using var stream = new MemoryStream();

        // Act
        tShort.Write(stream);

        // Assert
        var bytes = stream.ToArray();
        Assert.Equal(0x00, bytes[0]);
        Assert.Equal(0x01, bytes[1]);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(32767)]
    [InlineData(-32768)]
    public void RoundTrip_ReturnsOriginalValue(short value)
    {
        // Arrange
        var original = new TShort(value);
        using var stream = new MemoryStream();

        // Act
        original.Write(stream);
        stream.Position = 0;
        var result = TShort.Read(stream);

        // Assert
        Assert.Equal(value, result.Value);
    }
}