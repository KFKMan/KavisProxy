using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace KavisProxy.Core.Protocol.Packets;

public class LoginStart : Packet<LoginStartData>
{
    public int PacketID => 0;

    public LoginStartData Read(IByteBuffer buffer)
    {
        return new LoginStartData(buffer.ReadString());
    }

    public void Write(IByteBuffer buffer, LoginStartData value)
    {
        buffer.WriteString(value.Name);
    }
}

public record LoginStartData(string Name);