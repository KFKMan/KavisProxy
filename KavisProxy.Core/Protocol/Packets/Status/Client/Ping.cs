namespace KavisProxy.Core.Protocol.Packets;

public class Ping : Packet<PingData>
{
    public int PacketID => 1;

    public PingData Read(IByteBuffer buffer) => new(buffer.ReadLong());

    public void Write(IByteBuffer buffer, PingData value)
    {
        buffer.WriteLong(value.Payload);
    }
}

public record PingData(long Payload);
