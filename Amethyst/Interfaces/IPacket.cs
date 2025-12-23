namespace Amethyst.Interfaces;

/// <summary>
/// Defines a packet that can be serialized (written) to a stream.
/// </summary>
public interface IPacket
{
    int Id { get; }
    void Write(Stream stream);
    
    // Helper to get bytes directly (useful for wrapping in the main Packet container)
    byte[] ToBytes()
    {
        using var stream = new MemoryStream();
        Write(stream);
        return stream.ToArray();
    }
}