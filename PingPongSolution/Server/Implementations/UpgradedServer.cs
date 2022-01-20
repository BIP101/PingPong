using Common;
using log4net;
using Server.Abstractions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Implementations
{
    public class UpgradedServer<T> : IServer<Info<T>, TcpClient>
    {
        public IList<ClientInfo<TcpClient>> Clients { get; private set; }
        public ServerInfo ServerInfo { get; set; }

        private ILog _logger;
        private TcpListener _listener;
        private NetworkStream _stream;
        private byte[] _buffer;

        public UpgradedServer(ILog logger)
        {
            Clients = new List<ClientInfo<TcpClient>>();
            _logger = logger;
        }

        public async void Start()
        {
            // initalize server
            if (IPAddress.TryParse(ServerInfo.IP, out var address))
            {
                _listener = new TcpListener(address, ServerInfo.Port);
                _listener.Start();
                _buffer = new byte[ServerInfo.BufferSize];
                await AcceptCallback();
            }
            else
            {
                _logger.Error($"Received inccorrect ip, ip received: {ServerInfo.IP}");
                throw new InvalidCastException($"Received inccorrect ip, ip received: {ServerInfo.IP}");
            }
        }

        public async Task AcceptCallback()
        {
            // add client to client list and accept new clients
            Task<TcpClient> clientTask = _listener.AcceptTcpClientAsync();
            await clientTask;
            TcpClient client = clientTask.Result;
            _stream = client.GetStream();

            _logger.Debug($"Client has connected");

            Clients.Add(new ClientInfo<TcpClient>(client));
            await ReceiveCallback();
            await AcceptCallback();
        }

        public async Task ReceiveCallback()
        {
            int received = _stream.Read(_buffer, 0, _buffer.Length);
            byte[] dataBuffer = new byte[received];
            Array.Copy(_buffer, dataBuffer, received);

            Info<string> info = new Info<string>(Encoding.ASCII.GetString(dataBuffer));

            // resend info to client and get ready to receive new data
            _logger.Debug($"resending info to client, info is: {info.Information}");

            _stream.Write(dataBuffer, 0, dataBuffer.Length);
            Task asyncCallBack = new Task(() =>
            {
                var endWrite = new AsyncCallback(SendCallback);
            });

            await asyncCallBack;
        }

        public async void SendCallback(IAsyncResult asyncResult)
        {
            Task endWrite = new Task(() =>
            {
                _stream.EndWrite(asyncResult);
            });

            await endWrite;
        }
    }
}
