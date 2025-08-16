using Ambev.Ominia.Domain.Aggregates;
using Ambev.Ominia.Domain.Entities.Sales;
using Ambev.Ominia.Domain.Events.Sales;
using Ambev.Ominia.Domain.Handlers;

namespace Ambev.Ominia.Application.Features.Sales.Commands.CreateSale;

public class CreateSaleCommandHandler(ISaleRepository saleRepository, IMapper mapper, IEventSourcingHandler<SaleAggregate> eventSourcingHandler, IEventPublisher eventPublisher) : IRequestHandler<CreateSaleCommand, SaleDto>
{
    public async Task<SaleDto> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        // Create and save the sale entity
        var sale = mapper.Map<Sale>(command);
        var createdSale = await saleRepository.CreateAsync(sale, cancellationToken);
        
        // Create and save the sale aggregate for event sourcing
        var aggregateId = GetDeterministicGuid(createdSale.Id);
        var saleAggregate = SaleAggregate.Create(
            aggregateId,
            createdSale.Id,
            createdSale.SaleNumber,
            createdSale.Date,
            createdSale.Customer,
            createdSale.Branch,
            createdSale.Items);
            
        await eventSourcingHandler.SaveAsync(saleAggregate);
        
        // Publish the SaleCreated event
        var saleCreatedEvent = new SaleCreatedEvent(
            saleAggregate.Id,
            createdSale.Id,
            createdSale.SaleNumber,
            createdSale.Date,
            createdSale.Customer,
            createdSale.Branch,
            createdSale.Items);
            
        await eventPublisher.PublishAsync(saleCreatedEvent);
        
        var result = mapper.Map<SaleDto>(createdSale);
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