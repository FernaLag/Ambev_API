namespace Ambev.Ominia.Domain.Interfaces.Infrastructure;

/// <summary>
/// Interface for publishing domain events to external systems.
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Publishes an event asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of event to publish.</typeparam>
    /// <param name="event">The event to publish.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task PublishAsync<T>(T @event) where T : class;
}