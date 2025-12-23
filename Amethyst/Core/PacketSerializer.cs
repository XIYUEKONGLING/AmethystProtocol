using System.IO.Compression;
using Amethyst.Models;
using Amethyst.Types;

namespace Amethyst.Core;

public class PacketSerializer
{
    // 2^21 - 1 bytes (approx 2MB)
    private const int MaxPacketSize = 2097151;
    
    // Serverbound uncompressed limit (8MB)
    private const int MaxUncompressedSize = 8388608;

    private int _compressionThreshold = -1; // Negative means disabled

    public void SetCompressionThreshold(int threshold)
    {
        _compressionThreshold = threshold;
    }

    public Packet ReadPacket(Stream stream)
    {
        // 1. Read Packet Length
        // The protocol specifies the Length field must not be longer than 3 bytes.
        var length = ReadPacketLength(stream);

        if (length > MaxPacketSize)
        {
            throw new InvalidDataException($"Packet length {length} exceeds maximum allowed size of {MaxPacketSize}.");
        }

        // 2. Read the entire packet body into memory
        // We need the raw bytes to handle compression logic or simply to parse the ID and Data.
        var buffer = new byte[length];
        stream.ReadExactly(buffer);

        using var bufferStream = new MemoryStream(buffer);

        // 3. Handle Compression / No Compression
        if (_compressionThreshold >= 0)
        {
            return ReadCompressedPacket(bufferStream, length);
        }
        else
        {
            return ReadUncompressedPacket(bufferStream);
        }
    }

    public void WritePacket(Stream stream, Packet packet)
    {
        // 1. Prepare the raw packet (ID + Data)
        using var rawPacketStream = new MemoryStream();
        new TVarInt(packet.Id).Write(rawPacketStream);
        rawPacketStream.Write(packet.Data);
        var rawBytes = rawPacketStream.ToArray();

        // 2. Handle Compression / No Compression
        if (_compressionThreshold >= 0)
        {
            WriteCompressedPacket(stream, rawBytes);
        }
        else
        {
            WriteUncompressedPacket(stream, rawBytes);
        }
    }

    private Packet ReadUncompressedPacket(Stream stream)
    {
        var id = TVarInt.Read(stream).Value;
        
        // The rest of the stream is the data
        var dataLength = (int)(stream.Length - stream.Position);
        var data = new byte[dataLength];
        stream.ReadExactly(data);

        return new Packet(id, data);
    }

    private Packet ReadCompressedPacket(Stream stream, int packetLength)
    {
        var dataLength = TVarInt.Read(stream).Value;

        if (dataLength == 0)
        {
            // Uncompressed: size < threshold
            // The rest of the buffer is (Packet ID + Data)
            return ReadUncompressedPacket(stream);
        }
        else
        {
            // Compressed: size >= threshold
            // Validation: Uncompressed length limit
            if (dataLength > MaxUncompressedSize)
            {
                throw new InvalidDataException($"Decompressed length {dataLength} exceeds maximum allowed size of {MaxUncompressedSize}.");
            }

            // Decompress the remaining bytes
            using var decompressionStream = new ZLibStream(stream, CompressionMode.Decompress);
            
            // We need to read exactly 'dataLength' bytes
            var decompressedData = new byte[dataLength];
            var bytesRead = 0;
            while (bytesRead < dataLength)
            {
                var read = decompressionStream.Read(decompressedData, bytesRead, dataLength - bytesRead);
                if (read == 0) break;
                bytesRead += read;
            }

            if (bytesRead != dataLength)
            {
                throw new EndOfStreamException("Failed to read full decompressed packet.");
            }

            using var decompressedStream = new MemoryStream(decompressedData);
            return ReadUncompressedPacket(decompressedStream);
        }
    }

    private void WriteUncompressedPacket(Stream stream, byte[] rawBytes)
    {
        // Format: [Length][Packet ID + Data]
        new TVarInt(rawBytes.Length).Write(stream);
        stream.Write(rawBytes);
    }

    private void WriteCompressedPacket(Stream stream, byte[] rawBytes)
    {
        if (rawBytes.Length < _compressionThreshold)
        {
            // Format: [Packet Length][Data Length = 0][Packet ID + Data]
            
            // Calculate Packet Length: Length of (Data Length VarInt) + Length of (Raw Bytes)
            // Since Data Length is 0, it takes 1 byte (0x00)
            var packetLength = 1 + rawBytes.Length;

            new TVarInt(packetLength).Write(stream);
            new TVarInt(0).Write(stream); // Data Length = 0
            stream.Write(rawBytes);
        }
        else
        {
            // Format: [Packet Length][Data Length][Compressed (Packet ID + Data)]
            
            using var compressedStream = new MemoryStream();
            using (var zlib = new ZLibStream(compressedStream, CompressionLevel.Fastest, leaveOpen: true))
            {
                zlib.Write(rawBytes);
            }
            var compressedBytes = compressedStream.ToArray();

            var dataLengthVarIntSize = GetVarIntSize(rawBytes.Length);
            var packetLength = dataLengthVarIntSize + compressedBytes.Length;

            new TVarInt(packetLength).Write(stream);
            new TVarInt(rawBytes.Length).Write(stream); // Uncompressed Length
            stream.Write(compressedBytes);
        }
    }

    /// <summary>
    /// Reads a VarInt specifically for the Packet Length field.
    /// Throws if the encoding is longer than 3 bytes.
    /// </summary>
    private static int ReadPacketLength(Stream stream)
    {
        var value = 0;
        var position = 0;
        var byteCount = 0;

        while (true)
        {
            var byteRead = stream.ReadByte();
            if (byteRead == -1)
                throw new EndOfStreamException("End of stream reached while reading Packet Length.");

            byteCount++;
            if (byteCount > 3)
                throw new InvalidDataException("Packet Length field exceeded 3 bytes.");

            var currentByte = (byte)byteRead;
            value |= (currentByte & 0x7F) << position;

            if ((currentByte & 0x80) == 0) 
                break;

            position += 7;
        }

        return value;
    }

    private static int GetVarIntSize(int value)
    {
        var size = 0;
        var v = (uint)value;
        do
        {
            size++;
            v >>= 7;
        } while (v != 0);
        return size;
    }
}
