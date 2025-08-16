using Ambev.Ominia.Domain.Events.Sales;
using Rebus.Handlers;

namespace Ambev.Ominia.Application.Features.Sales.EventHandlers;

/// <summary>
/// Handler for processing SaleModifiedEvent messages from the message bus.
/// </summary>
public class SaleModifiedEventHandler(ILogger<SaleModifiedEventHandler> logger) : IHandleMessages<SaleModifiedEvent>
    {
    /// <summary>
    /// Handles the SaleModifiedEvent message.
    /// </summary>
    /// <param name="message">The SaleModifiedEvent to process.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Handle(SaleModifiedEvent message)
    {
        logger.LogInformation("Processing SaleModifiedEvent for Sale Number: {SaleNumber}, Aggregate ID: {AggregateId}",
            message.SaleNumber, message.Id);

        try
        {
            // Business logic for handling sale modification

            logger.LogInformation("Successfully processed SaleModifiedEvent for Sale Number: {SaleNumber}", message.SaleNumber);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing SaleModifiedEvent for Sale Number: {SaleNumber}: {Message}",
                message.SaleNumber, ex.Message);
            throw;
        }
    }
}