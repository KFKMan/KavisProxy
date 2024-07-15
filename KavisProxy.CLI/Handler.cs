using KavisProxy.Core.Protocol;
using KavisProxy.Core.Protocol.Packets;
using System;
using System.Collections.Generic;
using System.IO.Compression;
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

        public async Task HandleClient()
        {
            while (true)
            {
                var packet = await PacketReader.GetPacket();

                if (!ConnectionData.Handshake)
                {
                    var handshake = new Handshake().Read(packet.Data);
                    ConnectionData.NextState = handshake.GetNextStateType();
                    ConnectionData.Handshake = true;
                    continue;
                }

                if(ConnectionData.NextState != HandshakeData.NextStateEnum.Unhandled)
                {
                    if(ConnectionData.NextState == HandshakeData.NextStateEnum.Login)
                    {
                        var lgStart = new LoginStart();
                        if(packet.PacketID == lgStart.PacketID)
                        {

                        }
                    }
                    else if(ConnectionData.NextState == HandshakeData.NextStateEnum.Status)
                    {

                    }
                    ConnectionData.NextState = HandshakeData.NextStateEnum.Unhandled;
                    continue;
                }
            }
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
                //Decompression Mechanic (it can be more dry..)
                byte[] PacketIDDataRaw = buffer.ReadBytes(PacketLength);
                byte[] PacketIDDataDecompressed = ZLibHelper.Decompress(PacketIDDataRaw);
                int PacketId;
                using(ByteBuffer buff = new ByteBuffer(PacketIDDataDecompressed))
                {
                    PacketId = buff.ReadVarInt();
                }

                byte[] DataRaw = buffer.ReadBytes(DataLength);
                byte[] Data = ZLibHelper.Decompress(DataRaw);

                return new PacketData(PacketId, new ByteBuffer(Data));
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

    public class ZLibHelper
    {
        public static byte[] Decompress(byte[] data,CompressionLevel level = CompressionLevel.Optimal)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (ZLibStream stream = new ZLibStream(ms, CompressionLevel.Optimal))
                {
                    ByteBuffer buffer = new ByteBuffer(stream);
                    return buffer.ToArray();
                }
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
        public bool Handshake = false;
        public HandshakeData.NextStateEnum NextState = HandshakeData.NextStateEnum.Unhandled;
    }
}
