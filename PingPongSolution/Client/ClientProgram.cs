using Client.Implementations;
using Client.Orchestrators;
using log4net;
using System;

namespace Client
{
    public class ClientProgram
    {
        public static void Main(string[] args)
        {
            var logger = LogManager.GetLogger("logger");
            UpgradedClient<string> client = new UpgradedClient<string>(logger);
            SocketClient<string> socketClient = new SocketClient<string>(logger);

            ClientOrchestrator<UpgradedClient<string>> clientOrchestrator = new ClientOrchestrator<UpgradedClient<string>>(client, logger);
            clientOrchestrator.Start();

            ClientOrchestrator<SocketClient<string>> socketClientOrchestrator = new ClientOrchestrator<SocketClient<string>>(socketClient, logger);
            //socketClientOrchestrator.Start();

            Console.ReadLine();
        }
    }
}
