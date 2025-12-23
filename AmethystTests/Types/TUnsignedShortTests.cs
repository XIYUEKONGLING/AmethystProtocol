using Amethyst.Types;

namespace AmethystTests.Types;

public class TUnsignedShortTests
{
    [Fact]
    public void Write_IsBigEndian()
    {
        // Arrange
        ushort value = 65535; // 0xFFFF
        var tUShort = new TUnsignedShort(value);
        using var stream = new MemoryStream();

        // Act
        tUShort.Write(stream);

        // Assert
        var bytes = stream.ToArray();
        Assert.Equal(0xFF, bytes[0]);
        Assert.Equal(0xFF, bytes[1]);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(65535)]
    public void RoundTrip_ReturnsOriginalValue(ushort value)
    {
        // Arrange
        var original = new TUnsignedShort(value);
        using var stream = new MemoryStream();

        // Act
        original.Write(stream);
        stream.Position = 0;
        var result = TUnsignedShort.Read(stream);

        // Assert
        Assert.Equal(value, result.Value);
    }
}