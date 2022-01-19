using Client;
using log4net;
using Server;
using System;
using System.Configuration;
using System.Net.Sockets;

namespace PingPongSolution
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //get logger
            var loggerName = ConfigurationManager.AppSettings["loggerName"];
            var logger = LogManager.GetLogger(loggerName);

            SocketServer server = new SocketServer("server", 8200, 5, 1024, logger);
            server.Start();

            SocketClient client = new SocketClient(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp), logger, server.ServerInfo.Port);
            client.Start();

            SocketClient client2 = new SocketClient(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp), logger, server.ServerInfo.Port);
            client2.Start();

            SocketClient client3 = new SocketClient(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp), logger, server.ServerInfo.Port);
            client3.Start();
            Console.ReadLine();
        }
    }
}
