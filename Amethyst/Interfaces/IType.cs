namespace Amethyst.Interfaces;

public interface IType<TSelf> where TSelf : IType<TSelf>
{
    /// <summary>
    /// Writes the type to the provided stream.
    /// </summary>
    void Write(Stream stream);

    /// <summary>
    /// Reads the type from the provided stream.
    /// </summary>
    static abstract TSelf Read(Stream stream);

    /// <summary>
    /// Converts the type to a byte array.
    /// </summary>
    byte[] ToBytes()
    {
        using var stream = new MemoryStream();
        Write(stream);
        return stream.ToArray();
    }
}