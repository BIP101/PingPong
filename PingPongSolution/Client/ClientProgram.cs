using Client.Orchestrators;
using System;

namespace Client
{
    public class ClientProgram
    {
        public static void Main(string[] args)
        {
            SocketClientOrchestrator socketClientOrchestrator = new SocketClientOrchestrator();
            socketClientOrchestrator.Start();

            Console.ReadLine();
        }
    }
}
