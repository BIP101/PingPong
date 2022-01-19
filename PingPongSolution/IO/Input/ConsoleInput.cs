using IO.Input.Abstractions;
using log4net;
using System;
using System.IO;

namespace IO.Input
{
    public class ConsoleInput : IInput<string>
    {
        private ILog _logger;

        public ConsoleInput(ILog logger)
        {
            _logger = logger;
        }

        public string GetInput()
        {
            try
            {
                var input = Console.ReadLine();
                _logger.Debug($"Got input, Input: {input}");
                return input;
            }
            catch (IOException e)
            {
                _logger.Error(e);
            }

            return null;
        }
    }
}
