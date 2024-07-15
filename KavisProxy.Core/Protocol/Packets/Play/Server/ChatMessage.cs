namespace KavisProxy.Core.Protocol.Packets;

public class ChatMessage : Packet<ChatMessageData>
{
    public int PacketID = 0x02;

    int Packet.PacketID => throw new NotImplementedException();

    public ChatMessageData Read(IByteBuffer buffer)
    {
        return new ChatMessageData(buffer.ReadString(), buffer.ReadByte());
    }

    public void Write(IByteBuffer buffer, ChatMessageData value)
    {
        buffer.WriteString(value.JSONData);
        buffer.WriteByte(value.Position);
    }
}

public record ChatMessageData(string JSONData, byte Position)
{

    public enum PositionType : int
    {
        /// <summary>
        /// ChatBox
        /// </summary>
        Chat = 0,
        /// <summary>
        /// ChatBox
        /// </summary>
        SystemMessage = 1,
        AboveHotbar = 2
    }
}
