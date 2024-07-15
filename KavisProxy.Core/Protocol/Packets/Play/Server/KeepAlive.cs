using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KavisProxy.Core.Protocol.Packets;

public class KeepAlive : Packet<KeepAliveData>
{
    public int PacketID = 0x00;

    int Packet.PacketID => throw new NotImplementedException();

    public KeepAliveData Read(IByteBuffer buffer)
    {
        return new KeepAliveData(buffer.ReadVarInt());
    }

    public void Write(IByteBuffer buffer, KeepAliveData value)
    {
        buffer.WriteVarInt(value.KeepAliveId);
    }
}

public record KeepAliveData(int KeepAliveId);
