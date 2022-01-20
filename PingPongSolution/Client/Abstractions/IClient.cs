using Common.Abstractions;
using System.Net.Sockets;

namespace Client.Abstractions
{
    public interface IClient<T>
    {
        int ServerPort { get; }
        public string ServerIP { get; }
        Socket Socket { get; }
        void Start(string serverIP, int serverPort);
        void SendInfo(T infoToSend);
        void ReceiveInfo(int dataLength);
        T ParseInfo(byte[] infoToParse);
    }
}
