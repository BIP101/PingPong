using IO.Output.Abstractions;
using log4net;
using System;

namespace IO.Output
{
    public class ConsoleOutput<T> : IOutput<T>
    {
        private ILog _logger;

        public ConsoleOutput(ILog logger)
        {
            _logger = logger;
        }

        public void DisplayOutput(T output)
        {
            var outputStringFormat = output.ToString();
            _logger.Debug($"Displaying output, Output: {outputStringFormat}");
            Console.WriteLine(outputStringFormat);
        }
    }
}
