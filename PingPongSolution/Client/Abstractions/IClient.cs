using System.Collections.Generic;
using System.Net.Sockets;

namespace Client.Abstractions
{
    public interface IClient<T>
    {
        int ServerPort { get; }
        string ServerIP { get; }
        Stack<string> ReceivedInfo { get; }
        bool Start(string serverIP, int serverPort);
        void SendInfo(T infoToSend);
        void ReceiveInfo(int dataLength);
        void ParseInfo(byte[] infoToParse);
        bool IsConnected();
    }
}
