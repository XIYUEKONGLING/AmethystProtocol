using System.Text;
using Amethyst.Types;

namespace AmethystTests.Types;

public class TStringTests
{
    [Fact]
    public void Write_NormalString_EncodesCorrectly()
    {
        // Arrange
        var value = "Hello";
        var tString = new TString(value);
        using var stream = new MemoryStream();

        // Act
        tString.Write(stream);

        // Assert
        stream.Position = 0;
        var length = TVarInt.Read(stream);
        var buffer = new byte[length];
        stream.ReadExactly(buffer);
        
        Assert.Equal(5, length.Value);
        Assert.Equal("Hello", Encoding.UTF8.GetString(buffer));
    }

    [Fact]
    public void Read_RoundTrip_ReturnsOriginalValue()
    {
        // Arrange
        var value = "Minecraft 1.21";
        var tString = new TString(value);
        using var stream = new MemoryStream();
        tString.Write(stream);
        stream.Position = 0;

        // Act
        var result = TString.Read(stream);

        // Assert
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void Write_ExceedsMaxLength_ThrowsArgumentException()
    {
        // Arrange
        var longString = new string('a', 32768); // Limit is 32767
        var tString = new TString(longString);
        using var stream = new MemoryStream();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => tString.Write(stream));
    }

    [Fact]
    public void Read_MultiByteCharacters_HandlesCorrectly()
    {
        // Arrange
        var value = "こんにちは"; // "Hello" in Japanese
        var tString = new TString(value);
        using var stream = new MemoryStream();
        tString.Write(stream);
        stream.Position = 0;

        // Act
        var result = TString.Read(stream);

        // Assert
        Assert.Equal(value, result.Value);
    }
}