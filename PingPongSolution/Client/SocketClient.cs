using Client.Abstractions;
using Common;
using Common.Abstractions;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class SocketClient : IClient<StringInfo>
    {
        public Socket Socket;
        private ILog _logger;

        public SocketClient(Socket socket, ILog logger)
        {
            _logger = logger;
            Socket = socket;
        }

        public void Start()
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void ConnectToServer(ServerInfo serverInfo)
        {
            throw new NotImplementedException();
        }

        public void SendInfo(IInfo<StringInfo> infoToSend)
        {
            throw new NotImplementedException();
        }

        public void ParseInfo(StringInfo infoToParse)
        {
            throw new NotImplementedException();
        }

        public StringInfo GetInfo()
        {
            throw new NotImplementedException();
        }
    }
}
