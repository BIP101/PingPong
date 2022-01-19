using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Output.Abstractions
{
    public interface IOutput<T>
    {
        void DisplayOutput(T output);
    }
}
