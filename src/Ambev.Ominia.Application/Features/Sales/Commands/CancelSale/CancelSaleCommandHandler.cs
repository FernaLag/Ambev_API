using Ambev.Ominia.Domain.Aggregates;
using Ambev.Ominia.Domain.Events.Sales;
using Ambev.Ominia.Domain.Handlers;

namespace Ambev.Ominia.Application.Features.Sales.Commands.CancelSale;

public class CancelSaleCommandHandler(ISaleRepository saleRepository, IMapper mapper, IEventSourcingHandler<SaleAggregate> eventSourcingHandler, IEventPublisher eventPublisher) : IRequestHandler<CancelSaleCommand, SaleDto>
{
    public async Task<SaleDto> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await saleRepository.GetByIdAsync(request.Id, cancellationToken);
        if (sale == null)
            throw new KeyNotFoundException($"Sale with ID {request.Id} not found");

        if (sale.IsActive())
        {
            var saleAggregateId = GetDeterministicGuid(sale.Id);
            var saleAggregate = await eventSourcingHandler.GetByIdAsync(saleAggregateId);
            if (saleAggregate.Version < 0)
                throw new InvalidOperationException($"Sale aggregate with ID {saleAggregateId} not found. Cannot cancel a sale that was never created.");

            if (saleAggregate.Id == Guid.Empty)
                {
                var idField = typeof(SaleAggregate).BaseType?.GetField("_id", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                idField?.SetValue(saleAggregate, saleAggregateId);
                }

            sale.Cancel();
            saleAggregate.CancelSale();

            await eventSourcingHandler.SaveAsync(saleAggregate);
            await saleRepository.UpdateAsync(sale, cancellationToken);

            await eventPublisher.PublishAsync(new SaleCancelledEvent(saleAggregateId));
        }

        return mapper.Map<SaleDto>(sale);
    }

    /// <summary>
    /// Generates a deterministic GUID based on the sale ID.
    /// </summary>
    private static Guid GetDeterministicGuid(int saleId)
    {
        return Guid.Parse($"00000000-0000-0000-0000-{saleId:D12}");
    }
}