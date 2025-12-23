using System.Text;
using Amethyst.Interfaces;

namespace Amethyst.Types;

public readonly struct String(string value) : IType<String>
{
    private const int MaxLength = 32767;

    public string Value { get; } = value;

    public void Write(Stream stream)
    {
        if (Value.Length > MaxLength)
        {
            throw new ArgumentException($"String length of {Value.Length} exceeds maximum allowed length of {MaxLength}.", nameof(Value));
        }

        var bytes = Encoding.UTF8.GetBytes(Value);
        new VarInt(bytes.Length).Write(stream);
        stream.Write(bytes);
    }

    public static String Read(Stream stream)
    {
        var length = VarInt.Read(stream).Value;

        // Max bytes calculation based on documentation: (32767 * 3) + 3
        // However, we check the VarInt value (length of bytes) first.
        // The protocol defines the limit on the number of characters (UTF-16 code units), not strictly bytes on read,
        // but to prevent massive allocations, we can sanity check the byte length.
        // 32767 * 3 = 98301 bytes max payload.
        if (length < 0 || length > MaxLength * 3)
        {
            throw new InvalidDataException($"String byte length {length} is out of bounds.");
        }

        Span<byte> buffer = length <= 1024 ? stackalloc byte[length] : new byte[length];
        stream.ReadExactly(buffer);

        var result = Encoding.UTF8.GetString(buffer);

        if (result.Length > MaxLength)
        {
            throw new InvalidDataException($"String length {result.Length} exceeds maximum allowed length of {MaxLength}.");
        }

        return new String(result);
    }

    public static implicit operator string(String s) => s.Value;
    public static implicit operator String(string s) => new(s);
}