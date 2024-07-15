namespace KavisProxy.Core.Protocol.Packets;

public class UpdateHealth : Packet<UpdateHealthData>
{
    public int PacketID => 0x06;

    public UpdateHealthData Read(IByteBuffer buffer)
    {
        return new UpdateHealthData(buffer.ReadFloat(), buffer.ReadVarInt(), buffer.ReadFloat());
    }

    public void Write(IByteBuffer buffer, UpdateHealthData value)
    {
        buffer.WriteFloat(value.Health);
        buffer.WriteVarInt(value.Food);
        buffer.WriteFloat(value.FoodSaturation);
    }
}

public record UpdateHealthData(float Health, int Food, float FoodSaturation);
    
