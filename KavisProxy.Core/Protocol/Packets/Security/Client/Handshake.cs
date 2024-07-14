using static KavisProxy.Core.Protocol.Packets.HandshakeData;

namespace KavisProxy.Core.Protocol.Packets;

public class Handshake : Packet<HandshakeData>
{
    public byte PacketID => 0;

    public HandshakeData Read(IByteBuffer buffer)
    {
        var protocolVersion = buffer.ReadVarInt();
        var serverAddress = buffer.ReadString();
        var serverPort = buffer.ReadUnsignedShort();
        var nextState = (NextStateEnum)buffer.ReadVarInt();

        return new(protocolVersion, serverAddress, serverPort, nextState);
    }

    public void Write(IByteBuffer buffer, HandshakeData value)
    {
        buffer.WriteVarInt(value.ProtocolVersion);
        buffer.WriteString(value.ServerAddress);
        buffer.WriteUnsignedShort(value.ServerPort);
        buffer.WriteVarInt((int)value.NextState);
    }
}

public record HandshakeData(int ProtocolVersion, string ServerAddress, ushort ServerPort, NextStateEnum NextState)
{
    public enum NextStateEnum
    {
        Status,
        Login,
        Unhandled
    }
}
