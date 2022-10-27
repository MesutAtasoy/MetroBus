using MetroBus.Abstraction.EventHandlers;
using MetroBus.Abstraction.Events;

namespace MetroBus.Abstraction.SubscriptionsManagers.Abstract;

public interface IEventBusSubscriptionsManager
{
    bool IsEmpty { get; }

    void AddSubscription<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;
    
    bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent;
    
    bool HasSubscriptionsForEvent(string eventName);
    
    Type GetEventTypeByName(string eventName);
    
    IEnumerable<Type> GetHandlersForEvent(string eventName);
    
    void Clear();
    
    string GetEventKey<T>();
}
