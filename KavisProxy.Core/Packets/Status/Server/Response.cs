using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KavisProxy.Core.Packets.Status.Client
{
    public class Response : PacketBase<NoData>
    {
        public Response() : base(0x00)
        {

        }
    }
}
