using System.Net.Sockets;
using Amethyst.Core;
using Amethyst.Interfaces;
using Amethyst.Models;
using Amethyst.Packets.Clientbound;
using Amethyst.Packets.Handshake;
using Amethyst.Packets.Serverbound;
using Amethyst.Structs;

namespace Example.Network;

public class ProtocolClient : IDisposable
{
    private const int ProtocolVersion = 767; // Protocol 767
    private readonly string _host;
    private readonly int _port;
    private TcpClient? _client;
    private NetworkStream? _stream;
    private readonly PacketSerializer _serializer;

    public ProtocolClient(string host, int port)
    {
        _host = host;
        _port = port;
        _serializer = new PacketSerializer();
    }

    public async Task ConnectAsync()
    {
        _client = new TcpClient();
        
        // Set timeouts to avoid hanging
        _client.ReceiveTimeout = 5000;
        _client.SendTimeout = 5000;

        await _client.ConnectAsync(_host, _port);
        _stream = _client.GetStream();
    }

    public void PerformHandshake()
    {
        if (_stream is null) throw new InvalidOperationException("Not connected.");

        var handshake = new HandshakePacket(
            ProtocolVersion,
            _host,
            (ushort)_port,
            ProtocolState.Status
        );

        SendPacket(handshake);
    }

    public ServerStatus RequestStatus()
    {
        if (_stream is null) throw new InvalidOperationException("Not connected.");

        // 1. Send Request
        SendPacket(new StatusRequestPacket());

        // 2. Read Response (Loop until we get packet 0x00)
        while (true)
        {
            var packet = _serializer.ReadPacket(_stream);
            if (packet.Id == 0x00)
            {
                using var ms = new MemoryStream(packet.Data);
                var response = StatusResponsePacket.Read(ms);
                return response.Status;
            }
        }
    }

    public long PerformPing()
    {
        if (_stream is null) throw new InvalidOperationException("Not connected.");

        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        SendPacket(new PingRequestPacket(timestamp));

        // Read until we get Pong (0x01)
        while (true)
        {
            var packet = _serializer.ReadPacket(_stream);
            if (packet.Id == 0x01)
            {
                using var ms = new MemoryStream(packet.Data);
                var response = PongResponsePacket.Read(ms);
                
                // Calculate round-trip time
                var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                return now - timestamp;
            }
        }
    }

    private void SendPacket(IPacket packet)
    {
        if (_stream is null) return;
        var rawPacket = new Packet(packet.Id, packet.ToBytes());
        _serializer.WritePacket(_stream, rawPacket);
    }

    public void Dispose()
    {
        _stream?.Dispose();
        _client?.Dispose();
        GC.SuppressFinalize(this);
    }
}
