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
    public class UpgradedClient<T> : IClient<Info<T>>
    {
        public int ServerPort { get; private set; }
        public string ServerIP { get; private set; }
        public TcpClient TCPClient { get; private set; }
        public Stack<string> ReceivedInfo { get; }

        private ILog _logger;
        private NetworkStream _stream;

        public UpgradedClient(ILog logger)
        {
            _logger = logger;
            ReceivedInfo = new Stack<string>();
            TCPClient = new TcpClient();
        }

        public bool Start(string serverIP, int serverPort)
        {
            while (!TCPClient.Connected)
            {
                try
                {
                    IPAddress.TryParse(serverIP, out var address);
                    TCPClient.Connect(address, serverPort);

                    //if succesful, save parameters, initalize stream
                    ServerPort = serverPort;
                    ServerIP = serverIP;
                    _stream = TCPClient.GetStream();
                    return true;
                }
                catch (SocketException e)
                {
                    _logger.Warn($"Client failed to connect to server! Exception: {e}");
                }
            }

            return false;
        }

        public void SendInfo(Info<T> infoToSend)
        {
            byte[] dataBuffer = Encoding.ASCII.GetBytes(infoToSend.Information.ToString());
            _stream.Write(dataBuffer, 0, dataBuffer.Length);

            // since client received info the same length it sends
            ReceiveInfo(dataBuffer.Length);
        }

        public void ReceiveInfo(int dataLength)
        {
            byte[] receivedData = new byte[dataLength];
            _stream.Read(receivedData, 0, receivedData.Length);
            Array.Copy(receivedData, receivedData, receivedData.Length);
            ParseInfo(receivedData);
        }

        public void ParseInfo(byte[] infoToParse)
        {
            var parsedInfo = Encoding.ASCII.GetString(infoToParse);
            ReceivedInfo.Push(parsedInfo);

            _logger.Debug($"Client received info, info is: {parsedInfo}");
        }

        public bool IsConnected()
        {
            return TCPClient.Connected;
        }
    }
}
