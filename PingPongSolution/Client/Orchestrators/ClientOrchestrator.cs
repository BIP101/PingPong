using Client.Abstractions;
using Client.Implementations;
using Common;
using IO.Input;
using IO.Input.Abstractions;
using IO.Output;
using IO.Output.Abstractions;
using log4net;
using System;
using System.Configuration;

namespace Client.Orchestrators
{
    public class ClientOrchestrator<T>
        where T : IClient<Info<string>>
    {
        private T _client;
        private ILog _logger;
        private IInput<string> _input;
        private IOutput<string> _output;

        public ClientOrchestrator(T client, ILog logger)
        {
            _logger = logger;
            _input = new ConsoleInput(_logger);
            _output = new ConsoleOutput<string>(_logger);
            _client = client;
        }

        public void Start()
        {
            InitializeClientProperties();

            while (_client.IsConnected())
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
                _client.Start(parsedInput[0], int.Parse(parsedInput[1]));
                if (_client.IsConnected())
                {
                    _output.DisplayOutput($"Client is up and connect to server: {_client.ServerIP}:{_client.ServerPort}");
                }
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
            var input = new Info<string>(_input.GetInput());

            _client.SendInfo(input);
        }

        public void GetServerOutput()
        {
            if (_client.ReceivedInfo.TryPeek(out var serverOutput))
            {
                _output.DisplayOutput($"received output from server: {serverOutput}");
            }
            else
            {
                _output.DisplayOutput($"Client did not receive anything!");
            }
        }
    }
}
