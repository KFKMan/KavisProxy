using System;
using System.Collections.Generic;
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

public class JoinGame
{
    public int PacketID => 0x01;
}

/// <summary>
/// 
/// </summary>
/// <param name="EntityID">it's handled as raw Int on Network</param>
/// <param name="Gamemode"></param>
public record JoinGameData(int EntityID, byte Gamemode, byte Dimension)
{
    public enum GamemodeType : byte
    {
        Survival = 0,
        Creative = 1,
        Adventure = 2,
        Spectator = 3,
        Unhandled = 4
        //Looks like there is a Flag mechanic for Hardcore
#warning Need look
    }

    public enum DimensionType : byte
    {

    }
}
