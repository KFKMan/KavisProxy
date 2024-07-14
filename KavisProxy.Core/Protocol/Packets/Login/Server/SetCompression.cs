namespace KavisProxy.Core.Protocol.Packets;

public class SetCompression : Packet<SetCompressionData>
{
    public byte PacketID => 3;

    public SetCompressionData Read(IByteBuffer buffer)
    {
        return new SetCompressionData(buffer.ReadVarInt());
    }

    public void Write(IByteBuffer buffer, SetCompressionData value)
    {
        buffer.WriteVarInt(value.Threshold);
    }
}

public class SetCompressionData
{
    public SetCompressionData(int threshold)
    {
        Threshold = threshold;
    }

    public int Threshold;
}
