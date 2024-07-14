using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KavisProxy.Core.Packets
{
    //Clientbound = Which CLIENT GET
    //Serverbound = Which SERVER GET

    public class PacketBase<T> where T : class
    {
        public PacketBase(byte packetid)
        {
            PacketId = packetid;
        }

        public virtual T Read(IByteBuffer buffer)
        {
            return default(T);
        }

        public virtual void Write(IByteBuffer buffer,T value)
        {

        }

        public byte PacketId;
    }

    public class NoData
    {

    }
}
