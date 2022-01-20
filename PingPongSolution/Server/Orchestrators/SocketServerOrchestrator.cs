using IO.Input;
using IO.Input.Abstractions;
using IO.Output;
using IO.Output.Abstractions;
using log4net;
using Server.Implementations;
using System;
using System.Configuration;

namespace Server.Orchestrators
{
    public class SocketServerOrchestrator
    {
        private SocketServer _socketServer;
        private ILog _logger;
        private IInput<string> _input;
        private IOutput<string> _output;

        public SocketServerOrchestrator()
        {
            var loggerName = ConfigurationManager.AppSettings["loggerName"]; // fix this
            _logger = LogManager.GetLogger("loggerName");
            _input = new ConsoleInput(_logger);
            _output = new ConsoleOutput<string>(_logger);
        }

        public void Start()
        {
            InitializeServerProperties();
            _socketServer.Start();
        }

        public void InitializeServerProperties()
        {
            _output.DisplayOutput($"Enter server IP, Port, back log and buffer size," +
                $"all seperated by a ':',  example: 127.0.0.1:6666:5:1024");
            var input = _input.GetInput();

            var parsedInput = input.Split(':');
            try
            {
                _socketServer = new SocketServer(parsedInput[0], int.Parse(parsedInput[1])
                    , int.Parse(parsedInput[2]), int.Parse(parsedInput[3]), _logger);
                _output.DisplayOutput($"Server is up and listening on: {_socketServer.ServerInfo.IP}:{_socketServer.ServerInfo.Port}");
            }
            catch (Exception e)
            {
                _output.DisplayOutput($"input is of wrong type, got: {input}, try again!");
                _logger.Error(e);
                InitializeServerProperties();
            }
        }
    }
}
