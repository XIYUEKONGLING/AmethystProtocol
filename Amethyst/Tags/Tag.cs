using System.Text;
using Amethyst.Tags.Arrays;
using Amethyst.Tags.Collections;
using Amethyst.Tags.Primitives;
using Amethyst.Types;

namespace Amethyst.Tags;

public abstract class Tag
{
    public string? Name { get; set; }
    public abstract TagType Type { get; }

    /// <summary>
    /// Writes the tag payload (excluding ID and Name).
    /// </summary>
    public abstract void WritePayload(Stream stream);

    /// <summary>
    /// Reads the tag payload (excluding ID and Name).
    /// </summary>
    public abstract void ReadPayload(Stream stream);

    /// <summary>
    /// Writes the full tag: ID (optional), Name (optional), and Payload.
    /// </summary>
    public void Write(Stream stream, bool writeId = true, bool writeName = true)
    {
        if (writeId)
        {
            stream.WriteByte((byte)Type);
        }

        if (writeName)
        {
            if (Name is null) 
                throw new InvalidOperationException("Tag name cannot be null when writing name.");
            
            // NBT Names are standard UTF-8 strings prefixed by an unsigned short length
            var nameBytes = Encoding.UTF8.GetBytes(Name);
            new TUnsignedShort((ushort)nameBytes.Length).Write(stream);
            stream.Write(nameBytes);
        }

        WritePayload(stream);
    }

    public static Tag Create(TagType type) => type switch
    {
        TagType.End => new TagEnd(),
        TagType.Byte => new TagByte(),
        TagType.Short => new TagShort(),
        TagType.Int => new TagInt(),
        TagType.Long => new TagLong(),
        TagType.Float => new TagFloat(),
        TagType.Double => new TagDouble(),
        TagType.ByteArray => new TagByteArray(),
        TagType.String => new TagString(),
        TagType.List => new TagList(),
        TagType.Compound => new TagCompound(),
        TagType.IntArray => new TagIntArray(),
        TagType.LongArray => new TagLongArray(),
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown NBT Tag Type")
    };
}