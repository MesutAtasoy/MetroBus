using MetroBus.Abstraction;
using MetroBus.Abstraction.SubscriptionsManagers.Abstract;
using MetroBus.Extensions;
using MetroBus.RabbitMQ.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace MetroBus.RabbitMQ;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRabbitMQEventBus(this IServiceCollection serviceCollection,
        Action<MetroBusRabbitMQOptions> options)
    {
        var metroBusRabbitMqOptions = new MetroBusRabbitMQOptions();
        options?.Invoke(metroBusRabbitMqOptions);

        Check.IsNull(metroBusRabbitMqOptions, nameof(metroBusRabbitMqOptions));
        
        serviceCollection.AddSingleton<IEventBus, MetroBusRabbitMQ>(sp =>
        {
            var rabbitMqPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
            var logger = sp.GetRequiredService<ILogger<MetroBusRabbitMQ>>();
            var eventBusSubscriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

            return new MetroBusRabbitMQ(rabbitMqPersistentConnection,
                eventBusSubscriptionsManager,
                logger,
                metroBusRabbitMqOptions.ExchangeName,
                metroBusRabbitMqOptions.QueueName,
                metroBusRabbitMqOptions.RetryCount);
        });

        serviceCollection.AddRabbitMQPersistentConnection(metroBusRabbitMqOptions.Connection);


        return serviceCollection;
    }

    private static IServiceCollection AddRabbitMQPersistentConnection(this IServiceCollection serviceCollection,
        MetroBusRabbitMQConnectionOptions connectionOptions)
    {
        Check.IsNull(connectionOptions, nameof(connectionOptions));
        Check.IsNullOrEmpty(connectionOptions.Host, nameof(connectionOptions.Host));
        Check.IsNullOrEmpty(connectionOptions.Password, nameof(connectionOptions.Password));
        Check.IsNullOrEmpty(connectionOptions.Username, nameof(connectionOptions.Username));
        
        serviceCollection.AddSingleton<IRabbitMQPersistentConnection>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

            var factory = new ConnectionFactory()
            {
                HostName = connectionOptions.Host, // "localhost",
                DispatchConsumersAsync = true,
                Password = connectionOptions.Password, // "guest",
                UserName = connectionOptions.Username // "guest",
            };

            return new DefaultRabbitMQPersistentConnection(factory, logger);
        });

        return serviceCollection;
    }
}