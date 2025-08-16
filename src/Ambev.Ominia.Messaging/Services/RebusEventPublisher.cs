using Ambev.Ominia.Domain.Interfaces.Infrastructure;
using Microsoft.Extensions.Logging;
using Rebus.Bus;

namespace Ambev.Ominia.Messaging.Services;

public class RebusEventPublisher(IBus bus, ILogger<RebusEventPublisher> logger) : IEventPublisher
    {
    public async Task PublishAsync<T>(T @event) where T : class
    {
        try
        {
            logger.LogInformation("Publishing event {EventType}: {Event}", typeof(T).Name, @event);
            await bus.Publish(@event);
            logger.LogInformation("Successfully published event {EventType}", typeof(T).Name);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error publishing event {EventType}: {Message}", typeof(T).Name, ex.Message);
            // Next step should be to implement retry logic or fallback mechanisms
        }
    }
}