using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Input.Abstractions
{
    public interface IInput<T>
    {
        T GetInput();
    }
}
