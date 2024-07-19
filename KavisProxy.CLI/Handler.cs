using Hook;
using KavisProxy.Core.Protocol;
using KavisProxy.Core.Protocol.Packets;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static KavisProxy.CLI.HookHelper;
using SOCKET = nint;

namespace KavisProxy.CLI
{
	public class HookHelper
    {
		public static IPAddress UintToIPAddress(uint ipAddressAsUint)
		{
			byte[] bytes = BitConverter.GetBytes(ipAddressAsUint);
			Array.Reverse(bytes);
			return new IPAddress(bytes);
		}

		public enum AddressFamily
		{
			AppleTalk = 0x11,
			BlueTooth = 0x20,
			InterNetworkv4 = 2,
			InterNetworkv6 = 0x17,
			Ipx = 4,
			Irda = 0x1a,
			NetBios = 0x11,
			Unknown = 0
		}

		public enum ProtocolType
		{
			BlueTooth = 3,
			ReliableMulticast = 0x71,
			Tcp = 6,
			Udp = 0x11
		}

		[StructLayout(LayoutKind.Sequential, Size = 16)]
		public unsafe struct sockaddr_in
		{
			public const int Size = 16;

			public short sin_family;
			public ushort sin_port;
			
			public in_addr* sin_addr;
		}

		public struct in_addr
		{
			public uint S_addr;
			public struct _S_un_b
			{
				public byte s_b1, s_b2, s_b3, s_b4;
			}
			public _S_un_b S_un_b;
			public struct _S_un_w
			{
				public ushort s_w1, s_w2;
			}
			public _S_un_w S_un_w;
		}

		public enum SocketType
		{
			Unknown,
			Stream,
			DGram,
			Raw,
			Rdm,
			SeqPacket
		}
	}

	public unsafe class HookSystem
    {
        public HookSystem()
        {
			Base = Interop.GetModuleHandle("ws2_32");
            WSConnect = (delegate* unmanaged<SOCKET, sockaddr_in*, int, void>)((IntPtr)Interop.GetProcAddress(Base, "connect")).ToPointer();
            ConnectHook = new HookFunction(WSConnect, (delegate* unmanaged<SOCKET, sockaddr_in*, int, void>)&ConnectHandler);
        }

        //void connect(SOCKET socket, sockaddr* addr, int namelen)

        public nint Base;
        public HookFunction ConnectHook;

        public unsafe delegate* unmanaged<SOCKET, sockaddr_in*, int, void> WSConnect;

		[UnmanagedCallersOnly]
		public static void ConnectHandler(SOCKET socket, sockaddr_in* sockaddress, int length)
        {
            var IPAsUInt = sockaddress->sin_addr->S_addr;

			IPAddress ip = HookHelper.UintToIPAddress(IPAsUInt);
			int port = sockaddress->sin_port; //ushort
            LogManger.WriteLine($"Connection Request Getted {socket.ToString()} | {ip}:{port}");
            if(Sockets.TryGetValue(socket,out Handler server))
            {
                //There is already connection, just redirection
            }
            else
            {
                var handler = new Handler(ip.ToString(), port);
                var directto = handler.CreateFakeServer();


                //var lgserver = new Handler();

            }
        }

        public static Dictionary<nint, Handler> Sockets = new();
	}

    public class LoginServer
    {
        public LoginServer()
        {
            Random.Shared.NextBytes(VerifyToken);
            var rsa = RSA.Create(1024);
            PublicKey = rsa.ExportRSAPublicKey();
            PrivateKey = rsa.ExportRSAPrivateKey();
		}

        public byte[] VerifyToken = new byte[4];
        public byte[] PublicKey;
        public byte[] PrivateKey;
    }

    public class Handler
    {
        private TcpListener FakeServer;
        private LoginServer FakeServerCrypter = new();

        private NetworkStream Stream;
        private string TargetIP;
        private int TargetPort;

        public Handler(string targetIP,int targetPORT)
        {
            ConnectionData = new();
            PacketReader = new(Stream,ConnectionData);
            TargetIP = targetIP;
            TargetPort = targetPORT;
        }

        public IPEndPoint CreateFakeServer()
        {
            FakeServer = new TcpListener(IPAddress.Parse("127.0.0.1"), 0); //0 for auto selecting port;
            FakeServer.Start();
            return (IPEndPoint)FakeServer.LocalEndpoint;
        }



        private TcpClient RealServerConnection = new TcpClient();

        public async Task HandleServer()
        {
            RealServerConnection.Connect(TargetIP, TargetPort);
        }

        public async Task HandleClient()
        {
            HandleServer();
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
