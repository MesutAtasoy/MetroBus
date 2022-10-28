namespace MetroBus.Logging.Loggers;

public class MetroBusConsoleLogger<T> : IMetroBusLogger<T>
{
    public void Log(string message)
    {
        Console.WriteLine($"{message} | {DateTime.UtcNow} | {nameof(T)}");
    }
}