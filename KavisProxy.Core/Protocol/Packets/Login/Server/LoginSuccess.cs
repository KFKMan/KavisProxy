namespace KavisProxy.Core.Protocol.Packets;

public class LoginSuccess : Packet<LoginData>
{
    public byte PacketID => 2;

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

public class LoginData
{
    public LoginData(string uuid, string username)
    {
        UUID = uuid;
        Username = username;
    }

    /// <summary>
    /// UUID as string with hyphens.
    /// </summary>
    public string UUID;
    public string Username;
}
