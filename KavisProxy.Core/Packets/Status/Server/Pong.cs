namespace KavisProxy.Core.Packets.Status.Client
{
    public class Pong : PacketBase<PongData>
    {
        public Pong() : base(0x01)
        {

        }

        public override PongData Read(IByteBuffer buffer)
        {
            return new PongData(buffer.ReadLong());
        }

        public override void Write(IByteBuffer buffer, PongData value)
        {
            buffer.WriteLong(value.Payload);
        }
    }

    public class PongData
    {
        public PongData(long payload)
        {
            Payload = payload;
        }

        public long Payload;
    }
}
