using MetroBus.Abstraction.EventHandlers;
using MetroBus.Abstraction.Events;

namespace MetroBus.Abstraction;

public interface IEventBus
{
    void Publish(IntegrationEvent @event);
    
    Task PublishAsync(IntegrationEvent @event);

    void Subscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;

    Task ProcessEvent(string eventName, string message);
}
