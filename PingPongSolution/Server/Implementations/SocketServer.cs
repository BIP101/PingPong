using Common;
using log4net;
using Server.Abstractions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server.Implementations
{
    public class SocketServer : IServer<StringInfo, Socket>
    {
        public IList<ClientInfo<Socket>> Clients { get; private set; }
        public ServerInfo ServerInfo { get; private set; }
        private ILog _logger;
        private Socket _socket;
        private byte[] _buffer;

        public SocketServer(string ip, int port, int backLog, int bufferSize, ILog logger)
        {
            ServerInfo = new ServerInfo(ip, port, backLog, bufferSize);
            _buffer = new byte[ServerInfo.BufferSize];
            Clients = new List<ClientInfo<Socket>>();
            _logger = logger;
        }

        public void Start()
        {
            // initalize server
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            if (IPAddress.TryParse(ServerInfo.IP, out var address))
            {
                _socket.Bind(new IPEndPoint(address, ServerInfo.Port));
                _socket.Listen(ServerInfo.BackLog);
                _socket.BeginAccept(new AsyncCallback(AcceptCallback), null);
            }
            else
            {
                _logger.Error($"Received inccorrect ip, ip received: {ServerInfo.IP}");
                throw new InvalidCastException($"Received inccorrect ip, ip received: {ServerInfo.IP}");
            }


        }

        public void AcceptCallback(IAsyncResult asyncResult)
        {
            // add client to client list and accept new clients
            Socket clientSocket = _socket.EndAccept(asyncResult);

            _logger.Debug($"Client has connected");

            Clients.Add(new ClientInfo<Socket>(clientSocket));
            clientSocket.BeginReceive(_buffer, 0, ServerInfo.BufferSize, SocketFlags.None,
                new AsyncCallback(ReceiveCallback), clientSocket);
            _socket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        public void ReceiveCallback(IAsyncResult asyncResult)
        {
            // received data from client
            Socket clientSocket = asyncResult.AsyncState as Socket;
            int received = clientSocket.EndReceive(asyncResult);
            byte[] dataBuffer = new byte[received];
            Array.Copy(_buffer, dataBuffer, received);

            StringInfo stringInfo = new StringInfo(Encoding.ASCII.GetString(dataBuffer));

            // resend info to client and get ready to receive new data
            _logger.Debug($"resending info to client, info is: {stringInfo.Information}");

            clientSocket.BeginSend(dataBuffer, 0, dataBuffer.Length, SocketFlags.None,
                new AsyncCallback(SendCallback), clientSocket);
            clientSocket.BeginReceive(_buffer, 0, ServerInfo.BufferSize, SocketFlags.None,
               new AsyncCallback(ReceiveCallback), clientSocket);
        }

        public void SendCallback(IAsyncResult asyncResult)
        {
            Socket clientSocket = asyncResult.AsyncState as Socket;
            clientSocket.EndSend(asyncResult);
        }
    }
}
