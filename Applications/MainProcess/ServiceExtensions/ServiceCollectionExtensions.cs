using Application.Common.Models;
using MainProcess.EventHandlers;
using MetroBus.Abstraction.EventHandlers;
using MetroBus.Abstraction.SubscriptionsManagers;
using MetroBus.Abstraction.SubscriptionsManagers.Abstract;
using MetroBus.RabbitMQ;
using MetroBus.RabbitMQ.Options;
using Microsoft.Extensions.DependencyInjection;

namespace MainProcess.ServiceExtensions;

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

    public static IServiceCollection RegisterHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IIntegrationEventHandler<DataCaptureEvent>, DataCaptureEventHandler>();

        return serviceCollection;
    }
}