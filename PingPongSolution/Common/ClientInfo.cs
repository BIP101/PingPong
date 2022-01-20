using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ClientInfo<T>
    {
        public T Info { get;}

        public ClientInfo(T info)
        {
            Info = info;
        }
    }
}
