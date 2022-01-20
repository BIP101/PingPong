using Client.Implementations;
using IO.Input;
using IO.Input.Abstractions;
using IO.Output;
using IO.Output.Abstractions;
using log4net;
using System;
using System.Configuration;

namespace Client.Orchestrators
{
    public class SocketClientOrchestrator
    {
        private SocketClient _socketClient;
        private ILog _logger;
        private IInput<string> _input;
        private IOutput<string> _output;

        public SocketClientOrchestrator()
        {
            var loggerName = ConfigurationManager.AppSettings["loggerName"]; // fix this
            _logger = LogManager.GetLogger("loggerName");
            _input = new ConsoleInput(_logger);
            _output = new ConsoleOutput<string>(_logger);
            _socketClient = new SocketClient(_logger);
        }

        public void Start()
        {
            InitializeClientProperties();

            while (_socketClient.Socket.Connected)
            {
                GetInfoToSend();
                GetServerOutput();
            }
        }

        public void InitializeClientProperties()
        {
            _output.DisplayOutput($"Enter the ip and port of the server you'd like this client to join, Example: 127.0.0.1:6666");
            var input = _input.GetInput();
            var parsedInput = input.Split(':');

            try
            {
                _socketClient.Start(parsedInput[0], int.Parse(parsedInput[1]));
                _output.DisplayOutput($"Client is up and connect to server: {_socketClient.ServerIP}:{_socketClient.ServerPort}");
            }
            catch (Exception e)
            {
                _output.DisplayOutput($"input is of wrong type, got: {input}, try again!");
                _logger.Error(e);
                InitializeClientProperties();
            }
        }

        public void GetInfoToSend()
        {
            _output.DisplayOutput($"Enter info to send to server");
            var input = new Common.StringInfo(_input.GetInput());

            _socketClient.SendInfo(input);
        }

        public void GetServerOutput()
        {
            if (_socketClient.ReceivedInfo.TryPeek(out var serverOutput))
            {
                _output.DisplayOutput($"received output from server: {serverOutput.Information}");
            }
            else
            {
                _output.DisplayOutput($"Client did not receive anything!");
            }
        }
    }
}
