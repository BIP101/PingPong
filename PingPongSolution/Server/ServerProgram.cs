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

            ServerOrchestrator<UpgradedServer<string>, TcpClient> socketServerOrchestrator = new ServerOrchestrator<UpgradedServer<string>, TcpClient>(server, logger);
            socketServerOrchestrator.Start();

            Console.ReadLine();
        }
    }
}
