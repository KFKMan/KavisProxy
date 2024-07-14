using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KavisProxy.Core.Packets.Status.Server
{
    public class Request : PacketBase<NoData>
    {
        public Request() : base(0x00)
        {

        }
    }
}
