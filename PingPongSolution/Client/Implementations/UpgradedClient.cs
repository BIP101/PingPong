using Client.Abstractions;
using Common;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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

        public async void Start(string serverIP, int serverPort)
        {
            while (!TCPClient.Connected)
            {
                try
                {
                    IPAddress.TryParse(serverIP, out var address);
                    ServerPort = serverPort;
                    ServerIP = serverIP;

                    await TCPClient.ConnectAsync(address, serverPort);
                    _stream = TCPClient.GetStream();
                }
                catch (SocketException e)
                {
                    _logger.Warn($"Client failed to connect to server! Exception: {e}");
                }
            }
        }

        public void SendInfo(Info<T> infoToSend)
        {
            var writer = new StreamWriter(_stream);
            writer.AutoFlush = true;

            byte[] dataBuffer = Encoding.ASCII.GetBytes(infoToSend.Information.ToString());
            writer.WriteLineAsync(infoToSend.Information.ToString());

            // since client received info the same length it sends
            ReceiveInfo();
        }

        public async void ReceiveInfo()
        {
            await Task.Delay(100);
            var reader = new StreamReader(_stream);
            var response = await reader.ReadLineAsync();
            ParseInfo(response);
        }

        public void ParseInfo(string infoToParse)
        {
            ReceivedInfo.Push(infoToParse);

            _logger.Debug($"Client received info, info is: {infoToParse}");
        }

        public bool IsConnected()
        {
            return TCPClient.Connected;
        }
    }
}
