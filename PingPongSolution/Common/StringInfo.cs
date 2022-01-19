using Common.Abstractions;

namespace Common
{
    public class StringInfo : IInfo<string>
    {
        public string Information { get; private set; }

        public StringInfo(string information)
        {
            Information = information;
        }
    }
}
