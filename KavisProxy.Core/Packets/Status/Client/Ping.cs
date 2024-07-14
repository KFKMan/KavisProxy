namespace KavisProxy.Core.Packets.Status.Server
{
    public class Ping : PacketBase<PingData>
    {
        public Ping() : base(0x01)
        {

        }

        public override PingData Read(IByteBuffer buffer)
        {
            return new PingData(buffer.ReadLong());
        }

        public override void Write(IByteBuffer buffer, PingData value)
        {
            buffer.WriteLong(value.Payload);
        }
    }

    public class PingData
    {
        public PingData(long payload)
        {
            Payload = payload;
        }

        public long Payload;
    }
}
