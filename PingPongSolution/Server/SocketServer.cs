using Client;
using Client.Abstractions;
using Common;
using log4net;
using Server.Abstractions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class SocketServer : IServer<StringInfo>
    {
        public IList<IClient<StringInfo>> Clients { get; private set; }
        public ServerInfo ServerInfo { get; private set; }
        private ILog _logger;
        private Socket _socket;
        private byte[] _buffer;

        public SocketServer(string name, int port, int backLog, int bufferSize, ILog logger)
        {
            ServerInfo = new ServerInfo();
            ServerInfo.Name = name;
            ServerInfo.Port = port;
            ServerInfo.BackLog = backLog;
            ServerInfo.BufferSize = bufferSize;

            _buffer = new byte[ServerInfo.BufferSize];
            Clients = new List<IClient<StringInfo>>();
            _logger = logger;
        }

        public void Start()
        {
            // initalize server
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(new IPEndPoint(IPAddress.Any, ServerInfo.Port));
            _socket.Listen(ServerInfo.BackLog);
            _socket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        public void AcceptCallback(IAsyncResult asyncResult)
        {
            // add client to client list and accept new clients
            Socket clientSocket = _socket.EndAccept(asyncResult);

            _logger.Debug($"Client has connected");
            Console.WriteLine("Client has connected");

            Clients.Add(new SocketClient(clientSocket, _logger, ServerInfo.Port));
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
            _logger.Debug($"resending info to client, info is: {stringInfo}");
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

        public void SendData(StringInfo Data)
        {
            throw new NotImplementedException();
        }
    }
}
