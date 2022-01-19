using System;
using log4net;
using System.Configuration;

namespace PingPongSolution
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //get logger
            var loggerName = ConfigurationManager.AppSettings["loggerName"];
            var logger = LogManager.GetLogger(loggerName);

            logger.Debug($"test");
        }
    }
}
