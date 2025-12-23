using System.Buffers.Binary;
using System.Text;
using Amethyst.Types;

namespace Amethyst.Tags;

public class NbtString(string value = "") : NbtTag
{
    public string Value { get; set; } = value;
    public override NbtTagType Type => NbtTagType.String;

    public override void WritePayload(Stream stream)
    {
        var bytes = Encoding.UTF8.GetBytes(Value);
        new TUnsignedShort((ushort)bytes.Length).Write(stream);
        stream.Write(bytes);
    }

    public override void ReadPayload(Stream stream)
    {
        var len = TUnsignedShort.Read(stream).Value;
        Span<byte> buffer = len <= 1024 ? stackalloc byte[len] : new byte[len];
        stream.ReadExactly(buffer);
        Value = Encoding.UTF8.GetString(buffer);
    }
}

public class NbtByteArray(byte[]? value = null) : NbtTag
{
    public byte[] Value { get; set; } = value ?? [];
    public override NbtTagType Type => NbtTagType.ByteArray;

    public override void WritePayload(Stream stream)
    {
        new TInt(Value.Length).Write(stream);
        stream.Write(Value);
    }

    public override void ReadPayload(Stream stream)
    {
        var len = TInt.Read(stream).Value;
        Value = new byte[len];
        stream.ReadExactly(Value);
    }
}

public class NbtIntArray(int[]? value = null) : NbtTag
{
    public int[] Value { get; set; } = value ?? [];
    public override NbtTagType Type => NbtTagType.IntArray;

    public override void WritePayload(Stream stream)
    {
        new TInt(Value.Length).Write(stream);
        Span<byte> buffer = stackalloc byte[4];
        foreach (var i in Value)
        {
            BinaryPrimitives.WriteInt32BigEndian(buffer, i);
            stream.Write(buffer);
        }
    }

    public override void ReadPayload(Stream stream)
    {
        var len = TInt.Read(stream).Value;
        Value = new int[len];
        Span<byte> buffer = stackalloc byte[4];
        for (var i = 0; i < len; i++)
        {
            stream.ReadExactly(buffer);
            Value[i] = BinaryPrimitives.ReadInt32BigEndian(buffer);
        }
    }
}

public class NbtLongArray(long[]? value = null) : NbtTag
{
    public long[] Value { get; set; } = value ?? [];
    public override NbtTagType Type => NbtTagType.LongArray;

    public override void WritePayload(Stream stream)
    {
        new TInt(Value.Length).Write(stream);
        Span<byte> buffer = stackalloc byte[8];
        foreach (var l in Value)
        {
            BinaryPrimitives.WriteInt64BigEndian(buffer, l);
            stream.Write(buffer);
        }
    }

    public override void ReadPayload(Stream stream)
    {
        var len = TInt.Read(stream).Value;
        Value = new long[len];
        Span<byte> buffer = stackalloc byte[8];
        for (var i = 0; i < len; i++)
        {
            stream.ReadExactly(buffer);
            Value[i] = BinaryPrimitives.ReadInt64BigEndian(buffer);
        }
    }
}
