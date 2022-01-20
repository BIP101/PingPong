using log4net;
using Server.Implementations;
using Server.Orchestrators;
using System;
using System.Net.Sockets;

namespace Server
{
    internal class ServerProgram
    {
        public static void Main(string[] args)
        {
            var logger = LogManager.GetLogger("logger");
            UpgradedServer<string> server = new UpgradedServer<string>(logger);
            SocketServer<string> socketServer = new SocketServer<string>(logger);

            ServerOrchestrator<UpgradedServer<string>, TcpClient> serverOrchestrator = new ServerOrchestrator<UpgradedServer<string>, TcpClient>(server, logger);
            //serverOrchestrator.Start();

            ServerOrchestrator<SocketServer<string>, Socket> SocketServerOrchestrator = new ServerOrchestrator<SocketServer<string>, Socket>(socketServer, logger);
            SocketServerOrchestrator.Start();

            Console.ReadLine();
        }
    }
}
