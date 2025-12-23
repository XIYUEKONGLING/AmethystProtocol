using Amethyst.Tags;
using Amethyst.Tags.Collections;
using Amethyst.Tags.Primitives;

namespace AmethystTests.Tags;

public class TagIOTests
{
    [Fact]
    public void WriteAndRead_StandardNbt_ShouldIncludeRootName()
    {
        var root = new TagCompound { Name = "Root" };
        root.Add("child", new TagInt(1));

        using var stream = new MemoryStream();
        TagIO.Write(stream, root, isNetworkPacket: false);

        stream.Position = 0;
        var readRoot = TagIO.Read(stream, isNetworkPacket: false);

        Assert.Equal("Root", readRoot.Name);
        Assert.Equal(1, readRoot.GetInt("child"));
    }

    [Fact]
    public void WriteAndRead_NetworkNbt_ShouldSkipRootName()
    {
        var root = new TagCompound { Name = "IgnoredName" };
        root.Add("child", new TagInt(1));

        using var stream = new MemoryStream();
        // 1.20.2+ Network NBT
        TagIO.Write(stream, root, isNetworkPacket: true);

        stream.Position = 0;
        
        // Check raw bytes to ensure name is missing
        // Byte 0: Tag ID (10)
        // Byte 1: Start of payload (no name length/string bytes)
        var firstByte = stream.ReadByte();
        Assert.Equal((int)TagType.Compound, firstByte);
        
        // Reset to read via TagIO
        stream.Position = 0;
        var readRoot = TagIO.Read(stream, isNetworkPacket: true);

        Assert.Equal(string.Empty, readRoot.Name); // Name is lost/empty in network NBT
        Assert.Equal(1, readRoot.GetInt("child"));
    }
}