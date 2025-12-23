using System.Text;
using Amethyst.Types;

namespace Amethyst.Tags;

public abstract class NbtTag
{
    public string? Name { get; set; }
    public abstract NbtTagType Type { get; }

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
            if (Name is null) throw new InvalidOperationException("Tag name cannot be null when writing name.");
            
            // NBT Names are standard UTF-8 strings prefixed by an unsigned short length
            var nameBytes = Encoding.UTF8.GetBytes(Name);
            new TUnsignedShort((ushort)nameBytes.Length).Write(stream);
            stream.Write(nameBytes);
        }

        WritePayload(stream);
    }

    public static NbtTag Create(NbtTagType type) => type switch
    {
        NbtTagType.End => new NbtEnd(),
        NbtTagType.Byte => new NbtByte(),
        NbtTagType.Short => new NbtShort(),
        NbtTagType.Int => new NbtInt(),
        NbtTagType.Long => new NbtLong(),
        NbtTagType.Float => new NbtFloat(),
        NbtTagType.Double => new NbtDouble(),
        NbtTagType.ByteArray => new NbtByteArray(),
        NbtTagType.String => new NbtString(),
        NbtTagType.List => new NbtList(),
        NbtTagType.Compound => new NbtCompound(),
        NbtTagType.IntArray => new NbtIntArray(),
        NbtTagType.LongArray => new NbtLongArray(),
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown NBT Tag Type")
    };
}