namespace KavisProxy.Core.Protocol.Packets;

public class LoginSuccess : Packet<LoginData>
{
    public int PacketID => 2;

    public LoginData Read(IByteBuffer buffer)
    {
        return new LoginData(buffer.ReadString(), buffer.ReadString());
    }

    public void Write(IByteBuffer buffer, LoginData value)
    {
        buffer.WriteString(value.UUID);
        buffer.WriteString(value.Username);
    }
}

/// <summary>
/// 
/// </summary>
/// <param name="UUID">UUID as string with hyphens.</param>
/// <param name="Username"></param>
public record LoginData(string UUID, string Username);
