namespace KavisProxy.Core.Protocol.Packets;

public class SpawnPosition : Packet<SpawnPositionData>
{
    public int PacketID => 0x05;

    public SpawnPositionData Read(IByteBuffer buffer)
    {
        return new SpawnPositionData(buffer.ReadPosition());
    }

    public void Write(IByteBuffer buffer, SpawnPositionData value)
    {
        buffer.WritePosition(value.Position);
    }
}

public record SpawnPositionData(Vec3i Position);
