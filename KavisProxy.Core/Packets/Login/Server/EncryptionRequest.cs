namespace KavisProxy.Core.Packets.Login.Server
{
    public class EncryptionRequest : PacketBase<EncryptionRequestData>
    {
        public EncryptionRequest() : base(0x01)
        {

        }

        public override void Write(IByteBuffer buffer, EncryptionRequestData value)
        {
            buffer.WriteString(value.ServerId);
            buffer.WriteVarInt(value.PublicKeyLenght);
            buffer.WriteBytes(value.PublicKey);
            buffer.WriteVarInt(value.VerifyTokenLenght);
            buffer.WriteBytes(value.VerifyToken);
        }

        public override EncryptionRequestData Read(IByteBuffer buffer)
        {
            string serverid = buffer.ReadString();
            int publicKeyLenght = buffer.ReadVarInt();
            byte[] publicKey = buffer.ReadBytes(publicKeyLenght);
            int verifyTokenLenght = buffer.ReadVarInt();
            byte[] verifyToken = buffer.ReadBytes(verifyTokenLenght);

            return new EncryptionRequestData(serverid, publicKeyLenght,publicKey,verifyTokenLenght,verifyToken);
        }
    }

    public class EncryptionRequestData
    {
        public EncryptionRequestData(string serverId, int publicKeyLenght, byte[] publicKey, int verifyTokenLenght, byte[] verifyToken)
        {
            ServerId = serverId;
            PublicKeyLenght = publicKeyLenght;
            PublicKey = publicKey;
            VerifyTokenLenght = verifyTokenLenght;
            VerifyToken = verifyToken;
        }

        public string ServerId;
        public int PublicKeyLenght;
        public byte[] PublicKey;
        public int VerifyTokenLenght;
        public byte[] VerifyToken;
    }
}
