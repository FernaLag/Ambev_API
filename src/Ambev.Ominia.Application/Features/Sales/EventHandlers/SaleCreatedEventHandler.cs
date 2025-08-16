using Ambev.Ominia.Domain.Events.Sales;
using Rebus.Handlers;

namespace Ambev.Ominia.Application.Features.Sales.EventHandlers;

/// <summary>
/// Handler for processing SaleCreatedEvent messages from the message bus.
/// </summary>
public class SaleCreatedEventHandler(ILogger<SaleCreatedEventHandler> logger) : IHandleMessages<SaleCreatedEvent>
    {
    /// <summary>
    /// Handles the SaleCreatedEvent message.
    /// </summary>
    /// <param name="message">The SaleCreatedEvent to process.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Handle(SaleCreatedEvent message)
    {
        logger.LogInformation("Processing SaleCreatedEvent for Sale ID: {SaleId}, Aggregate ID: {AggregateId}",
            message.SaleId, message.Id);

        try
        {
            // Business logic for handling sale creation

            logger.LogInformation("Successfully processed SaleCreatedEvent for Sale ID: {SaleId}", message.SaleId);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing SaleCreatedEvent for Sale ID: {SaleId}: {Message}",
                message.SaleId, ex.Message);
            throw;
        }
    }
}