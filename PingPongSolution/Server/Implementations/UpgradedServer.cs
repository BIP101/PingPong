using Common;
using log4net;
using Server.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
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
            TcpClient client = await _listener.AcceptTcpClientAsync();
            _stream = client.GetStream();

            _logger.Debug($"Client has connected");

            Clients.Add(new ClientInfo<TcpClient>(client));
            await ReceiveCallback();
            await AcceptCallback();
        }

        public async Task ReceiveCallback()
        {
            var reader = new StreamReader(_stream);

            string received = await reader.ReadLineAsync();
            await SendCallBack(received);
        }

        public async Task SendCallBack(string received)
        {
            var writer = new StreamWriter(_stream);
            writer.AutoFlush = true;
            Info<string> info = new Info<string>(received);

            // resend info to client and get ready to receive new data
            _logger.Debug($"resending info to client, info is: {info.Information}");
            Console.WriteLine($"resending info to client, info is: {info.Information}");

            await writer.FlushAsync();
            await writer.WriteLineAsync(received);
            await ReceiveCallback();
        }
    }
}
