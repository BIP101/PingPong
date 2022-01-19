using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ServerInfo
    {
        public string Name { get; set; }
        public int Port { get; set; }
        public int BackLog { get; set; }
        public int BufferSize { get; set; }

    }
}
