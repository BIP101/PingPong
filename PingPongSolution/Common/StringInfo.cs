using Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
