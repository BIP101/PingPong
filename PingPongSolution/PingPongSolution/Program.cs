using Client.Implementations;
using Common;
using log4net;
using System;
using System.Configuration;

namespace PingPongSolution
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            //get logger
            var loggerName = ConfigurationManager.AppSettings["loggerName"];
            var logger = LogManager.GetLogger(loggerName);

            SocketClient client = new SocketClient(logger);
            client.Start("127.0.0.1", 8200);
            client.SendInfo(new StringInfo("hello"));

            SocketClient client2 = new SocketClient(logger);
            client2.Start("127.0.0.1", 8200);
            client2.SendInfo(new StringInfo("hello2"));

            SocketClient client3 = new SocketClient(logger);
            client3.Start("127.0.0.1", 8200);
            client3.SendInfo(new StringInfo("hello3"));

            Console.ReadLine();
        }
    }
}
