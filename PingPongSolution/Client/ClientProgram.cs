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
            ClientOrchestrator<UpgradedClient<string>> socketClientOrchestrator = new ClientOrchestrator<UpgradedClient<string>>(client, logger);
            socketClientOrchestrator.Start();

            Console.ReadLine();
        }
    }
}
