using Amethyst.Types;

namespace Amethyst.Tags;

public class NbtEnd : NbtTag
{
    public override NbtTagType Type => NbtTagType.End;
    public override void WritePayload(Stream stream) { } // No payload
    public override void ReadPayload(Stream stream) { }
}

public class NbtByte(sbyte value = 0) : NbtTag
{
    public sbyte Value { get; set; } = value;
    public override NbtTagType Type => NbtTagType.Byte;

    public override void WritePayload(Stream stream) => new TByte(Value).Write(stream);
    public override void ReadPayload(Stream stream) => Value = TByte.Read(stream);
}

public class NbtShort(short value = 0) : NbtTag
{
    public short Value { get; set; } = value;
    public override NbtTagType Type => NbtTagType.Short;

    public override void WritePayload(Stream stream) => new TShort(Value).Write(stream);
    public override void ReadPayload(Stream stream) => Value = TShort.Read(stream);
}

public class NbtInt(int value = 0) : NbtTag
{
    public int Value { get; set; } = value;
    public override NbtTagType Type => NbtTagType.Int;

    public override void WritePayload(Stream stream) => new TInt(Value).Write(stream);
    public override void ReadPayload(Stream stream) => Value = TInt.Read(stream);
}

public class NbtLong(long value = 0) : NbtTag
{
    public long Value { get; set; } = value;
    public override NbtTagType Type => NbtTagType.Long;

    public override void WritePayload(Stream stream) => new TLong(Value).Write(stream);
    public override void ReadPayload(Stream stream) => Value = TLong.Read(stream);
}

public class NbtFloat(float value = 0) : NbtTag
{
    public float Value { get; set; } = value;
    public override NbtTagType Type => NbtTagType.Float;

    public override void WritePayload(Stream stream) => new TFloat(Value).Write(stream);
    public override void ReadPayload(Stream stream) => Value = TFloat.Read(stream);
}

public class NbtDouble(double value = 0) : NbtTag
{
    public double Value { get; set; } = value;
    public override NbtTagType Type => NbtTagType.Double;

    public override void WritePayload(Stream stream) => new TDouble(Value).Write(stream);
    public override void ReadPayload(Stream stream) => Value = TDouble.Read(stream);
}
