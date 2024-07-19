namespace KavisProxy.Core.Protocol.Packets;

public class EntityEquipment : Packet<EntityEquipmentData>
{
    public int PacketID => 0x04;

    public EntityEquipmentData Read(IByteBuffer buffer)
    {
        return new(buffer.ReadVarInt(), buffer.ReadShort(), new(buffer.ReadShort(), buffer.ReadByte(), buffer.ReadShort(),NBTTagCompound.Empty));
    }

    public void Write(IByteBuffer buffer, EntityEquipmentData value)
    {
        buffer.WriteVarInt(value.EntityId);
        buffer.WriteShort(value.EquipmentSlot);
        buffer.WriteShort(value.Item.ItemId);
        buffer.WriteByte(value.Item.Amount);
        buffer.WriteShort(value.Item.Meta);
    }
}

public record EntityEquipmentData(int EntityId, short EquipmentSlot, ItemStack Item);

public record ItemStack(short ItemId, byte Amount, short Meta, NBTTagCompound NBTTag);

public class NBTTagController
{
    public static NBTTagCompound Read(IByteBuffer buffer)
    {
        var bf = buffer.ReadByte();

        if(bf == 0)
        {
            return NBTTagCompound.Empty;
        }
        else
        {
            //Handling
            return NBTTagCompound.Empty;
        }
        
    }
}

public class NBTTagCompound
{
    public static readonly NBTTagCompound Empty = new();
}

