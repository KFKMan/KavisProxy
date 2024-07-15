namespace KavisProxy.Core.Protocol.Packets;

public class EncryptionRequest : Packet<EncryptionRequestData>
{
    public int PacketID => 1;

    public void Write(IByteBuffer buffer, EncryptionRequestData value)
    {
        buffer.WriteString(value.ServerID);
        buffer.WriteVarInt(value.PublicKeyLenght);
        buffer.WriteBytes(value.PublicKey);
        buffer.WriteVarInt(value.VerifyTokenLenght);
        buffer.WriteBytes(value.VerifyToken);
    }

    public EncryptionRequestData Read(IByteBuffer buffer)
    {
        var serverid = buffer.ReadString();
        var publicKeyLenght = buffer.ReadVarInt();
        var publicKey = buffer.ReadBytes(publicKeyLenght);
        var verifyTokenLenght = buffer.ReadVarInt();
        var verifyToken = buffer.ReadBytes(verifyTokenLenght);

        return new(serverid, publicKeyLenght,publicKey,verifyTokenLenght,verifyToken);
    }
}

public record EncryptionRequestData(string ServerID, int PublicKeyLenght, byte[] PublicKey, int VerifyTokenLenght, byte[] VerifyToken);