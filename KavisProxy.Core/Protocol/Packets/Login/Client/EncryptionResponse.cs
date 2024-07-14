namespace KavisProxy.Core.Protocol.Packets;

public class EncryptionResponse : Packet<EncryptionResponseData>
{
    public byte PacketID => 1;

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

public class EncryptionResponseData
{
    public EncryptionResponseData(int sharedSecretLength, byte[] sharedSecret, int verifyTokenLength, byte[] verifyToken)
    {
        SharedSecretLength = sharedSecretLength;
        SharedSecret = sharedSecret;
        VerifyTokenLength = verifyTokenLength;
        VerifyToken = verifyToken;
    }


    public int SharedSecretLength;
    public byte[] SharedSecret;
    public int VerifyTokenLength;
    public byte[] VerifyToken;
}
