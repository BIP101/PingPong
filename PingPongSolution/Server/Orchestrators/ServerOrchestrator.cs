using Common;
using IO.Input;
using IO.Input.Abstractions;
using IO.Output;
using IO.Output.Abstractions;
using log4net;
using Server.Abstractions;
using Server.Implementations;
using System;
using System.Configuration;

namespace Server.Orchestrators
{
    public class ServerOrchestrator<T, J>
        where T : IServer<Info<string>, J>
    {
        private T _server;
        private ILog _logger;
        private IInput<string> _input;
        private IOutput<string> _output;

        public ServerOrchestrator(T server, ILog logger)
        {
            _logger = logger;
            _server = server; 
            _input = new ConsoleInput(_logger);
            _output = new ConsoleOutput<string>(_logger);
        }

        public void Start()
        {
            InitializeServerProperties();
            _server.Start();
        }

        public void InitializeServerProperties()
        {
            _output.DisplayOutput($"Enter server IP, Port, back log and buffer size," +
                $"all seperated by a ':',  example: 127.0.0.1:6666:5:1024");
            var input = _input.GetInput();

            var parsedInput = input.Split(':');
            try
            {
                var serverInfo = new ServerInfo(parsedInput[0], int.Parse(parsedInput[1])
                    , int.Parse(parsedInput[2]), int.Parse(parsedInput[3]));

                _server.ServerInfo = serverInfo;
                //_server.Start();
                _output.DisplayOutput($"Server is up and listening on: {_server.ServerInfo.IP}:{_server.ServerInfo.Port}");
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
