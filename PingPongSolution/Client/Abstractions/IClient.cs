using Common;
using Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Abstractions
{
    public interface IClient<T>
    {
        ClientInfo ClientInfo { get; }
        void Start();
        void ConnectToServer(ServerInfo serverInfo);
        void SendInfo(IInfo<T> infoToSend);
        void ParseInfo(T infoToParse);
        T GetInfo();
    }
}
