using Amethyst.Core;
using Amethyst.Interfaces;
using Amethyst.Types;

namespace Amethyst.Packets.Handshake;

public enum ProtocolState
{
    Status = 1,
    Login = 2,
}

public class HandshakePacket(int protocolVersion, string serverAddress, ushort serverPort, ProtocolState nextState) : IPacket
{
    public int Id => 0x00;

    public int ProtocolVersion { get; } = protocolVersion;
    public string ServerAddress { get; } = serverAddress;
    public ushort ServerPort { get; } = serverPort;
    public ProtocolState NextState { get; } = nextState;

    public void Write(Stream stream)
    {
        new TVarInt(ProtocolVersion).Write(stream);
        new TString(ServerAddress).Write(stream);
        new TUnsignedShort(ServerPort).Write(stream);
        new TVarInt((int)NextState).Write(stream);
    }

    public static HandshakePacket Read(Stream stream)
    {
        var version = TVarInt.Read(stream);
        var address = TString.Read(stream);
        var port = TUnsignedShort.Read(stream);
        var state = TVarInt.Read(stream);

        return new HandshakePacket(version, address, port, (ProtocolState)state.Value);
    }
}