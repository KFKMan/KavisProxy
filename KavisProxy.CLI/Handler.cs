using KavisProxy.Core.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace KavisProxy.CLI
{
    public class Handler
    {
        private NetworkStream Stream;

        public Handler(NetworkStream stream)
        {
            Stream = stream;
            ConnectionData = new();
            PacketReader = new(Stream,ConnectionData);
        }

        public async Task Handle()
        {

        }

        public ConnectionData ConnectionData;
        public PacketReader PacketReader;
    }

    public class PacketReader
    {
        private NetworkStream Stream;
        private ConnectionData ConnectionData;
        

        public PacketReader(NetworkStream stream,ConnectionData connectionData)
        {
            Stream = stream;
            ConnectionData = connectionData;
        }

        public void SetStream(NetworkStream stream)
        {
            Stream = stream;
        }

        public void SetConnectionData(ConnectionData connectionData)
        {
            ConnectionData = connectionData;
        }

        public async ValueTask<IPacketData> GetPacket()
        {
            ByteBuffer buffer = new ByteBuffer(Stream, (int)Stream.Position); //Stream.Read only supports int, Don't dispose it's NetworkStream !
            if (ConnectionData.CompressionEnabled)
            {
                int PacketLength = buffer.ReadVarInt();
                int DataLength = buffer.ReadVarInt();
                //Decompression Mechanic
            }
            else
            {
                int Length = buffer.ReadVarInt();
                int PacketId = buffer.ReadVarInt();
                byte[] Data = buffer.ReadBytes(Length - buffer.GetReadedCount());
                return new PacketData(PacketId, new ByteBuffer(Data));
            }
        }
    }

    public interface IPacketData
    {
        int PacketID { get; }
        IByteBuffer Data { get; }
    }

    public class PacketData : IPacketData
    {
        public PacketData(int packetID, IByteBuffer data)
        {
            PacketID = packetID;
            Data = data;
        }

        public int PacketID { get; set; }
        public IByteBuffer Data { get; set; }

    }

    public class ConnectionData
    {
        public bool CompressionEnabled = false;
    }
}
