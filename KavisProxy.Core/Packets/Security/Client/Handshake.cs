using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KavisProxy.Core.Packets.Security.Server
{
    public class Handshake : PacketBase<HandshakeData>
    {
        public Handshake() : base(0x00)
        {

        }

        public override HandshakeData Read(IByteBuffer buffer)
        {
            var data = new HandshakeData(
                buffer.ReadVarInt(),
                buffer.ReadString(),
                buffer.ReadUnsignedShort(),
                buffer.ReadVarInt());

            return data;
        }

        public override void Write(IByteBuffer buffer, HandshakeData value)
        {
            buffer.WriteVarInt(value.ProtocolVersion);
            buffer.WriteString(value.ServerAddress);
            buffer.WriteUnsignedShort(value.ServerPort);
            buffer.WriteVarInt(value.NextState);
        }
    }

    public class HandshakeData
    {
        public HandshakeData(int protocolVersion,string serverAddress,ushort serverPort,int nextState)
        {
            ProtocolVersion = protocolVersion;
            ServerAddress = serverAddress;
            ServerPort = serverPort;
            NextState = nextState;
        }

        public int ProtocolVersion;
        public string ServerAddress;
        public ushort ServerPort;
        public int NextState;

        public enum NextStateEnum
        {
            Status,
            Login,
            Unhandled
        }

        public NextStateEnum GetNextState()
        {
            switch (NextState)
            {
                case 1:
                    return NextStateEnum.Status;
                case 2:
                    return NextStateEnum.Login;
                default:
                    return NextStateEnum.Unhandled;
            }
        }
    }
}
