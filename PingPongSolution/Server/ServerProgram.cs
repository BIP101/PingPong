using Server.Orchestrators;
using System;

namespace Server
{
    internal class ServerProgram
    {
        public static void Main(string[] args)
        {
            SocketServerOrchestrator socketServerOrchestrator = new SocketServerOrchestrator();
            socketServerOrchestrator.Start();

            Console.ReadLine();
        }
    }
}
