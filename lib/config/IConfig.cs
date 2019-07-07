namespace MarsRover.Config
{
    // Represents a configuration line read from the input file
    public interface IConfig<T>
    {
        T Value { get; }
    }
}