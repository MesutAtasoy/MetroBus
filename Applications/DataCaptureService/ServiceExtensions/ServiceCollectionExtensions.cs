using DataCaptureService.Options;
using MetroBus.Abstraction.SubscriptionsManagers;
using MetroBus.Abstraction.SubscriptionsManagers.Abstract;
using MetroBus.RabbitMQ;
using MetroBus.RabbitMQ.Options;
using Microsoft.Extensions.DependencyInjection;

namespace DataCaptureService.ServiceExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEventBus(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddRabbitMQEventBus(x =>
        {
            x.Connection = new MetroBusRabbitMQConnectionOptions
            {
                Host = "localhost",
                Password = "guest",
                Username = "guest"
            };
            x.ExchangeName = "metrobus";
            x.QueueName = "mainprocessing";
        });
        
        serviceCollection.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        
        return serviceCollection;
    }

    public static IServiceCollection AddApplicationSettings(this IServiceCollection serviceCollection,
        Action<ApplicationOption> action)
    {
        var option = new ApplicationOption();
        
        action?.Invoke(option);

        serviceCollection.AddSingleton(option);
        
        return serviceCollection;
    }
}