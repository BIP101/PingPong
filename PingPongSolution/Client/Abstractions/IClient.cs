using System.Collections.Generic;
using System.Net.Sockets;

namespace Client.Abstractions
{
    public interface IClient<T>
    {
        int ServerPort { get; }
        string ServerIP { get; }
        Stack<string> ReceivedInfo { get; }
        void Start(string serverIP, int serverPort);
        void SendInfo(T infoToSend);
        bool IsConnected();
    }
}
