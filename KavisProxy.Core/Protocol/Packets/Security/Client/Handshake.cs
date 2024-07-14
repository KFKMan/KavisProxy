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
        var nextState = buffer.ReadVarInt();

        return new(protocolVersion, serverAddress, serverPort, nextState);
    }

    public void Write(IByteBuffer buffer, HandshakeData value)
    {
        buffer.WriteVarInt(value.ProtocolVersion);
        buffer.WriteString(value.ServerAddress);
        buffer.WriteUnsignedShort(value.ServerPort);
        buffer.WriteVarInt(value.NextState);
    }
}

public record HandshakeData(int ProtocolVersion, string ServerAddress, ushort ServerPort, int NextState)
{
    public enum NextStateEnum : int
    {
        Status = 1,
        Login = 2,
        /// <summary>
        /// There is no really value like that
        /// </summary>
        Unhandled = 3
    }

    public NextStateEnum GetNextStateType()
    {
        switch (NextState)
        {
            case (int)NextStateEnum.Status:
                return NextStateEnum.Status;
            case (int)NextStateEnum.Login:
                return NextStateEnum.Login;
            default:
                return NextStateEnum.Unhandled;
        }
    }
}
