using Client.Abstractions;
using Common;
using Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class SocketClient : IClient<string>
    {
        public string Name { get; private set; }

        public SocketClient(string name)
        {
            Name = name;
        }

        public void ConnectToServer(ServerInfo serverInfo)
        {
            throw new NotImplementedException();
        }

        public string GetInfo()
        {
            throw new NotImplementedException();
        }

        public void ParseInfo(string infoToParse)
        {
            throw new NotImplementedException();
        }

        public void SendInfo(IInfo<string> infoToSend)
        {
            throw new NotImplementedException();
        }
    }
}
