namespace IO.Output.Abstractions
{
    public interface IOutput<T>
    {
        void DisplayOutput(T output);
    }
}
