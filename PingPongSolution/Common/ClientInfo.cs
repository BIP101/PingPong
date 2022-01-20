namespace Common
{
    public class ClientInfo<T>
    {
        public T Info { get; }

        public ClientInfo(T info)
        {
            Info = info;
        }
    }
}
