namespace Common
{
    public class Info<T>
    {
        public T Information { get; set; }

        public Info() { }
        public Info(T information)
        {
            Information = information;
        }
    }
}
