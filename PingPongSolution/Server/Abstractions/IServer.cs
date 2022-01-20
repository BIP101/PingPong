using Common;
using System;
using System.Collections.Generic;

namespace Server.Abstractions
{
    public interface IServer<T, J>
    {
        IList<ClientInfo<J>> Clients { get; }
        ServerInfo ServerInfo { get; set; }
        void Start();
    }
}
