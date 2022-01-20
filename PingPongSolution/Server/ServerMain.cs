using log4net;
using System;
using System.Configuration;

namespace Server
{
    public class ServerMain
    {
        public static void Main(string[] args)
        {
            var loggerName = ConfigurationManager.AppSettings["loggerName"]; // fix this
            var logger = LogManager.GetLogger("loggerName");

            SocketServer server = new SocketServer("server", "127.0.0.1", 8200, 5, 1024, logger);
            server.Start();

            Console.ReadLine();
        }
    }
}
