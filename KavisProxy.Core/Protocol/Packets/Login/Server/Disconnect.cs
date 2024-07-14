namespace KavisProxy.Core.Protocol.Packets;

public class Disconnect : Packet<Chat>
{
    public byte PacketID => 0;
    public Chat Read(IByteBuffer buffer) => new Chat(buffer.ReadString());
    public void Write(IByteBuffer buffer, Chat value) => buffer.WriteString(value.RawData);
}