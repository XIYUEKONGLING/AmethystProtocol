using System.Text;
using Amethyst.Types;

namespace Amethyst.Tags;

public static class NbtIO
{
    /// <summary>
    /// Reads a root NBT Compound from a stream.
    /// </summary>
    /// <param name="stream">The input stream.</param>
    /// <param name="isNetworkPacket">
    /// If true, adheres to 1.20.2+ Network NBT spec (Root Compound has no name).
    /// If false, adheres to Disk/Standard spec (Root Compound has name).
    /// </param>
    public static NbtCompound Read(Stream stream, bool isNetworkPacket = false)
    {
        var typeId = stream.ReadByte();
        if (typeId == -1) throw new EndOfStreamException();

        if ((NbtTagType)typeId != NbtTagType.Compound)
        {
            throw new InvalidDataException($"Root tag must be a TAG_Compound (ID 10), found ID {typeId}.");
        }

        var root = new NbtCompound();

        if (!isNetworkPacket)
        {
            // Standard NBT: Read Root Name
            var nameLen = TUnsignedShort.Read(stream).Value;
            if (nameLen > 0)
            {
                Span<byte> nameBuffer = nameLen <= 1024 ? stackalloc byte[nameLen] : new byte[nameLen];
                stream.ReadExactly(nameBuffer);
                root.Name = Encoding.UTF8.GetString(nameBuffer);
            }
            else
            {
                root.Name = string.Empty;
            }
        }
        else
        {
            // Network NBT (>= 1.20.2): Root name is skipped entirely
            root.Name = string.Empty;
        }

        root.ReadPayload(stream);
        return root;
    }

    /// <summary>
    /// Writes a root NBT Compound to a stream.
    /// </summary>
    /// <param name="stream">The output stream.</param>
    /// <param name="tag">The compound to write.</param>
    /// <param name="isNetworkPacket">
    /// If true, adheres to 1.20.2+ Network NBT spec (Root Compound has no name).
    /// If false, adheres to Disk/Standard spec (Root Compound has name).
    /// </param>
    public static void Write(Stream stream, NbtCompound tag, bool isNetworkPacket = false)
    {
        // Write ID
        stream.WriteByte((byte)NbtTagType.Compound);

        if (!isNetworkPacket)
        {
            // Standard NBT: Write Name
            var name = tag.Name ?? string.Empty;
            var nameBytes = Encoding.UTF8.GetBytes(name);
            new TUnsignedShort((ushort)nameBytes.Length).Write(stream);
            stream.Write(nameBytes);
        }
        else
        {
            // Network NBT (>= 1.20.2): Do NOT write name or name length
        }

        tag.WritePayload(stream);
    }
}
