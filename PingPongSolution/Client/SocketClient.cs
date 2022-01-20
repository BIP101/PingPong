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
        public string ServerIP { get; private set; }
        public Socket Socket { get; private set; }
        private ILog _logger;

        public SocketClient(ILog logger)
        {
            _logger = logger;
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Start(string serverIP, int serverPort)
        {
            while (!Socket.Connected)
            {
                try
                {
                    IPAddress.TryParse(serverIP, out var address);
                    Socket.Connect(address, serverPort);

                    //if succesful, save parameters
                    ServerPort = serverPort;
                    ServerIP = serverIP;
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
            ReceiveInfo(dataBuffer.Length);
        }

        public void ReceiveInfo(int dataLength)
        {
            byte[] receivedData = new byte[dataLength];
            Socket.Receive(receivedData);
            Array.Copy(receivedData, receivedData, receivedData.Length);
            ParseInfo(receivedData);
        }

        public StringInfo ParseInfo(byte[] infoToParse)
        {
            var parsedInfo = Encoding.ASCII.GetString(infoToParse);
            var stringInfo = new StringInfo(parsedInfo);

            _logger.Debug($"Client received info, info is: {parsedInfo}");
            Console.WriteLine($"received: {parsedInfo}");
            return stringInfo;
        }
    }
}
