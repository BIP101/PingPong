using Client.Abstractions;
using Common;
using log4net;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client.Implementations
{
    public class SocketClient : IClient<StringInfo>
    {
        public int ServerPort { get; private set; }
        public string ServerIP { get; private set; }
        public Socket Socket { get; private set; }
        public Stack<StringInfo> ReceivedInfo { get; }

        private ILog _logger;

        public SocketClient(ILog logger)
        {
            _logger = logger;
            ReceivedInfo = new Stack<StringInfo>();
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public bool Start(string serverIP, int serverPort)
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
                    return true;
                }
                catch (SocketException)
                {
                    _logger.Warn($"Client failed to connect to server!");
                }
            }

            return false;
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

        public void ParseInfo(byte[] infoToParse)
        {
            var parsedInfo = Encoding.ASCII.GetString(infoToParse);
            var stringInfo = new StringInfo(parsedInfo);
            ReceivedInfo.Push(stringInfo);

            _logger.Debug($"Client received info, info is: {parsedInfo}");
        }
    }
}
