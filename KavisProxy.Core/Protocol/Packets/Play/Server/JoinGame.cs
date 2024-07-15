namespace KavisProxy.Core.Protocol.Packets;

public class JoinGame : Packet<JoinGameData>
{
    public int PacketID => 0x01;

    public JoinGameData Read(IByteBuffer buffer)
    {
        return new JoinGameData(
            buffer.ReadInt(),
            buffer.ReadByte(),
            buffer.ReadByteSigned(),
            buffer.ReadByte(),
            buffer.ReadByte(),
            buffer.ReadString(),
            buffer.ReadBool()
            );
    }

    public void Write(IByteBuffer buffer, JoinGameData value)
    {
        buffer.WriteInt(value.EntityID);
        buffer.WriteByte(value.Gamemode);
        buffer.WriteByteSigned(value.Dimension);
        buffer.WriteByte(value.Difficulity);
        buffer.WriteByte(value.MaxPlayers);
        buffer.WriteString(value.LevelType);
        buffer.WriteBool(value.ReducedDebugInfo);
    }
}

/// <summary>
/// 
/// </summary>
/// <param name="EntityID">it's handled as raw Int on Network</param>
/// <param name="Gamemode"></param>
public record JoinGameData(int EntityID, byte Gamemode, sbyte Dimension, byte Difficulity, byte MaxPlayers, string LevelType, bool ReducedDebugInfo)
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

    public enum DimensionType : sbyte
    {
        Nether = -1,
        Overworld = 0,
        End = 1
    }

    public enum DiffucilityType : byte
    {
        Peaceful = 0,
        Easy = 1,
        Normal = 2,
        Hard = 3
    }

    //LevelTypeName
    //Default => default
    //Flat => flat
    //LargeBiomes => largeBiomes
    //Amplified => amplified
    //Default 1 1 => default_1_1
}
