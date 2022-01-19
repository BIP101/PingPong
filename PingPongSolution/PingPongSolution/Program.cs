using System;
using log4net;
using System.Configuration;
using Server;

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
            Console.ReadLine();
        }
    }
}
