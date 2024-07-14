using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KavisProxy.Core.Packets.Login.Server
{
    public class Disconnect : PacketBase<Chat>
    {
        public Disconnect() : base(0x00)
        {

        }

        public override Chat Read(IByteBuffer buffer)
        {
            return new Chat(buffer.ReadString());
        }

        public override void Write(IByteBuffer buffer, Chat value)
        {
            buffer.WriteString(value.RawData);
        }
    }
}
