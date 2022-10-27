namespace MetroBus.RabbitMQ.Options;

public class MetroBusRabbitMQOptions
{
    public string ExchangeName { get; set; }
    public string QueueName { get; set; }
    public int RetryCount { get; set; }
    public MetroBusRabbitMQConnectionOptions Connection { get; set; }
}

public class MetroBusRabbitMQConnectionOptions
{
    public string Host { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}