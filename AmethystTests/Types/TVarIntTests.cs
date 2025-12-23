using Amethyst.Types;

namespace AmethystTests.Types;

public class TVarIntTests
{
    [Theory]
    [InlineData(0, new byte[] { 0x00 })]
    [InlineData(1, new byte[] { 0x01 })]
    [InlineData(2, new byte[] { 0x02 })]
    [InlineData(127, new byte[] { 0x7f })]
    [InlineData(128, new byte[] { 0x80, 0x01 })]
    [InlineData(255, new byte[] { 0xff, 0x01 })]
    [InlineData(25565, new byte[] { 0xdd, 0xc7, 0x01 })]
    [InlineData(2097151, new byte[] { 0xff, 0xff, 0x7f })]
    [InlineData(2147483647, new byte[] { 0xff, 0xff, 0xff, 0xff, 0x07 })]
    [InlineData(-1, new byte[] { 0xff, 0xff, 0xff, 0xff, 0x0f })]
    [InlineData(-2147483648, new byte[] { 0x80, 0x80, 0x80, 0x80, 0x08 })]
    public void Write_EncodesCorrectly(int value, byte[] expectedBytes)
    {
        // Arrange
        var varInt = new TVarInt(value);
        using var stream = new MemoryStream();

        // Act
        varInt.Write(stream);

        // Assert
        Assert.Equal(expectedBytes, stream.ToArray());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2147483647)]
    [InlineData(-2147483648)]
    public void Read_RoundTrip_ReturnsOriginalValue(int value)
    {
        // Arrange
        var original = new TVarInt(value);
        using var stream = new MemoryStream();
        original.Write(stream);
        stream.Position = 0;

        // Act
        var result = TVarInt.Read(stream);

        // Assert
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void Read_TooManyBytes_ThrowsInvalidDataException()
    {
        // Arrange: 6 bytes with continue bit set (invalid for 32-bit VarInt)
        var invalidBytes = new byte[] { 0x80, 0x80, 0x80, 0x80, 0x80, 0x01 };
        using var stream = new MemoryStream(invalidBytes);

        // Act & Assert
        Assert.Throws<InvalidDataException>(() => TVarInt.Read(stream));
    }
}