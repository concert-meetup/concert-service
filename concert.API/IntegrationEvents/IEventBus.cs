using concert.API.IntegrationEvents.Events;

namespace concert.API.IntegrationEvents;

public interface IEventBus
{
    Task PublishAsync(IntegrationEvent @event);
}