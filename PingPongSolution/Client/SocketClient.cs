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
    public class SocketClient : IClient<string>
    {
        public ClientInfo ClientInfo { get; private set; }
        private Socket _socket;
        private ILog _logger;

        public SocketClient(string name, string address, int port, ILog logger)
        {
            ClientInfo.Name = name;
            ClientInfo.Address = address;
            ClientInfo.Port = port;
            _logger = logger; 
        }

        public void Start()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
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
