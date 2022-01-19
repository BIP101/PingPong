using Client.Abstractions;
using Common;
using System;
using System.Collections.Generic;

namespace Server.Abstractions
{
    public interface IServer<T>
    {
        IList<IClient<T>> Clients { get; }
        ServerInfo ServerInfo { get; }
        void Start();
        void AcceptCallback(IAsyncResult asyncResult);
        void ReceiveCallback(IAsyncResult asyncResult);
        void SendCallback(IAsyncResult asyncResult);
        void SendData(T Data);
    }
}
