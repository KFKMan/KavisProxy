namespace KavisProxy.Core.Protocol.Packets;

public class EncryptionResponse : Packet<EncryptionResponseData>
{
    public int PacketID => 1;

    public EncryptionResponseData Read(IByteBuffer buffer)
    {
        var sharedsecretlength = buffer.ReadVarInt();
        var sharedsecret = buffer.ReadBytes(sharedsecretlength);
        var verifytokenlength = buffer.ReadVarInt();
        var verifytoken = buffer.ReadBytes(verifytokenlength);

        return new EncryptionResponseData(sharedsecretlength, sharedsecret, verifytokenlength, verifytoken);
    }

    public void Write(IByteBuffer buffer, EncryptionResponseData value)
    {
        buffer.WriteVarInt(value.SharedSecretLength);
        buffer.WriteBytes(value.SharedSecret);
        buffer.WriteVarInt(value.VerifyTokenLength);
        buffer.WriteBytes(value.VerifyToken);
    }
}

public record EncryptionResponseData(int SharedSecretLength, byte[] SharedSecret, int VerifyTokenLength, byte[] VerifyToken);
