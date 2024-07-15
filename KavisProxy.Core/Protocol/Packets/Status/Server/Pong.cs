namespace KavisProxy.Core.Protocol.Packets;

public class Pong : Packet<PongData>
{
    public int PacketID => 1;

    public PongData Read(IByteBuffer buffer) => new(buffer.ReadLong());

    public void Write(IByteBuffer buffer, PongData value) => buffer.WriteLong(value.Payload);
}

public record PongData(long Payload);