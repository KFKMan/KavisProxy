namespace KavisProxy.Core.Protocol.Packets;

public class SetCompression : Packet<SetCompressionData>
{
    public int PacketID => 3;

    public SetCompressionData Read(IByteBuffer buffer)
    {
        return new SetCompressionData(buffer.ReadVarInt());
    }

    public void Write(IByteBuffer buffer, SetCompressionData value)
    {
        buffer.WriteVarInt(value.Threshold);
    }
}

public record SetCompressionData(int Threshold);
