using Client.Abstractions;
using Common;
using log4net;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

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

        public void SendInfo(StringInfo infoToSend)
        {
            byte[] dataBuffer = Encoding.ASCII.GetBytes(infoToSend.Information);
            Socket.Send(dataBuffer);

            // since client received info the same length it sends
            byte[] receivedData = new byte[dataBuffer.Length];
            Socket.Receive(receivedData);
            Array.Copy(receivedData, receivedData, receivedData.Length);
            Console.WriteLine($"received: {Encoding.ASCII.GetString(receivedData)}"); 
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
