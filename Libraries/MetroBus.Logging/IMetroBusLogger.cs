namespace MetroBus.Logging;

public interface IMetroBusLogger<T>
{
    void Log(string message);
}