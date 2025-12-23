using Amethyst.Types;

namespace AmethystTests.Types;

public class TVarLongTests
{
    [Theory]
    [InlineData(0, new byte[] { 0x00 })]
    [InlineData(1, new byte[] { 0x01 })]
    [InlineData(2, new byte[] { 0x02 })]
    [InlineData(127, new byte[] { 0x7f })]
    [InlineData(128, new byte[] { 0x80, 0x01 })]
    [InlineData(255, new byte[] { 0xff, 0x01 })]
    [InlineData(2147483647, new byte[] { 0xff, 0xff, 0xff, 0xff, 0x07 })]
    [InlineData(9223372036854775807, new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x7f })]
    [InlineData(-1, new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x01 })]
    [InlineData(-2147483648, new byte[] { 0x80, 0x80, 0x80, 0x80, 0xf8, 0xff, 0xff, 0xff, 0xff, 0x01 })]
    [InlineData(-9223372036854775808, new byte[] { 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x01 })]
    public void Write_EncodesCorrectly(long value, byte[] expectedBytes)
    {
        // Arrange
        var varLong = new TVarLong(value);
        using var stream = new MemoryStream();

        // Act
        varLong.Write(stream);

        // Assert
        Assert.Equal(expectedBytes, stream.ToArray());
    }

    [Fact]
    public void Read_TooManyBytes_ThrowsInvalidDataException()
    {
        // Arrange: 11 bytes with continue bit set
        var invalidBytes = Enumerable.Repeat((byte)0x80, 10).Append((byte)0x01).ToArray();
        using var stream = new MemoryStream(invalidBytes);

        // Act & Assert
        Assert.Throws<InvalidDataException>(() => TVarLong.Read(stream));
    }
}