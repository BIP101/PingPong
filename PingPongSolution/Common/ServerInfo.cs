namespace Common
{
    public class ServerInfo
    {
        public string IP { get; set; }
        public int Port { get; set; }
        public int BackLog { get; set; }
        public int BufferSize { get; set; }

        public ServerInfo(string iP, int port, int backLog, int bufferSize)
        {
            IP = iP;
            Port = port;
            BackLog = backLog;
            BufferSize = bufferSize;
        }
    }
}
