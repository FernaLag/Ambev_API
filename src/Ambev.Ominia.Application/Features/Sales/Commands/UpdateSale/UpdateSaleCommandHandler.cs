using Ambev.Ominia.Domain.Aggregates;
using Ambev.Ominia.Domain.Events.Sales;
using Ambev.Ominia.Domain.Exceptions;
using Ambev.Ominia.Domain.Handlers;

namespace Ambev.Ominia.Application.Features.Sales.Commands.UpdateSale;

public class UpdateSaleCommandHandler(
    ISaleRepository saleRepository,
    IMapper mapper,
    IEventSourcingHandler<SaleAggregate> eventSourcingHandler,
    IEventPublisher eventPublisher
    ) : IRequestHandler<UpdateSaleCommand, SaleDto>
{
    public async Task<SaleDto> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        // Get and update the sale entity
        var sale = await saleRepository.GetByIdAsync(command.Id, cancellationToken);
        mapper.Map(command, sale);
        var updatedSale = await saleRepository.UpdateAsync(sale, cancellationToken);

        // Get the aggregate by ID (using a deterministic GUID based on the sale ID)
        var aggregateId = GetDeterministicGuid(updatedSale.Id);
        
        SaleAggregate saleAggregate;
        try
        {
            // Try to get existing aggregate
            saleAggregate = await eventSourcingHandler.GetByIdAsync(aggregateId);
        }
        catch (AggregateNotFoundException)
        {
            // If aggregate doesn't exist, create it (for sales created via DataSeed)
            saleAggregate = SaleAggregate.Create(
                aggregateId,
                updatedSale.Id,
                updatedSale.SaleNumber,
                updatedSale.Date,
                updatedSale.Customer,
                updatedSale.Branch,
                updatedSale.Items);
            
            // Save the newly created aggregate
            await eventSourcingHandler.SaveAsync(saleAggregate);
            
            // Publish the SaleCreated event for the newly created aggregate
            var saleCreatedEvent = new SaleCreatedEvent(
                saleAggregate.Id,
                updatedSale.Id,
                updatedSale.SaleNumber,
                updatedSale.Date,
                updatedSale.Customer,
                updatedSale.Branch,
                updatedSale.Items);
            
            await eventPublisher.PublishAsync(saleCreatedEvent);
        }

        // Update the aggregate and save it
        saleAggregate.UpdateSale(
            updatedSale.SaleNumber,
            updatedSale.Date,
            updatedSale.Customer,
            updatedSale.Branch,
            updatedSale.Items);

        await eventSourcingHandler.SaveAsync(saleAggregate);

        // Publish the SaleModified event
        var saleModifiedEvent = new SaleModifiedEvent(
            saleAggregate.Id,
            updatedSale.SaleNumber,
            updatedSale.Date,
            updatedSale.Customer,
            updatedSale.Branch,
            updatedSale.Items);

        await eventPublisher.PublishAsync(saleModifiedEvent);

        var result = mapper.Map<SaleDto>(updatedSale);
        return result;
    }

    /// <summary>
    /// Generates a deterministic GUID based on the sale ID.
    /// </summary>
    private static Guid GetDeterministicGuid(int saleId)
    {
        return Guid.Parse($"00000000-0000-0000-0000-{saleId:D12}");
    }
}