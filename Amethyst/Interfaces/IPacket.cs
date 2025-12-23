namespace Amethyst.Interfaces;

/// <summary>
/// Represents a packet that can be sent to the client.
/// </summary>
public interface IOutgoingPacket
{
    int Id { get; }
    void Write(Stream stream);
}

/// <summary>
/// Represents a packet received from the client.
/// </summary>
public interface IIncomingPacket<out TSelf> where TSelf : IIncomingPacket<TSelf>
{
    static abstract int Id { get; }
    static abstract TSelf Read(Stream stream);
}