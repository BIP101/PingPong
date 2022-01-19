using Client.Abstractions;
using Common;
using Common.Abstractions;
using log4net;
using System;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    public class SocketClient : IClient<StringInfo>
    {
        public int ServerPort { get; private set; }
        public Socket Socket { get; private set; }
        private ILog _logger;

        public SocketClient(Socket socket, ILog logger, int serverPort)
        {
            ServerPort = serverPort;
            Socket = socket;
            _logger = logger;
        }

        public void Start()
        {
            while (!Socket.Connected)
            {
                try
                {
                    Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    Socket.Connect(IPAddress.Loopback, ServerPort);
                    Console.WriteLine("connected");
                }
                catch (SocketException)
                {
                    _logger.Warn($"Client failed to connect to server!");
                }
            }
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
