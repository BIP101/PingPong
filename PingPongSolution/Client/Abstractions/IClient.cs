using Common.Abstractions;
using System.Net.Sockets;

namespace Client.Abstractions
{
    public interface IClient<T>
    {
        int ServerPort { get; }
        Socket Socket { get; }
        void Start();
        void SendInfo(IInfo<T> infoToSend);
        void ParseInfo(T infoToParse);
        T GetInfo();
    }
}
