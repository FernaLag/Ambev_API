using Ambev.Ominia.Domain.Aggregates;
using Ambev.Ominia.Domain.Handlers;

namespace Ambev.Ominia.Application.Features.Sales.Commands.CancelItem;

/// <summary>
/// Cancels a specific item in a sale (event-sourced).
/// </summary>
public class CancelItemCommandHandler : IRequestHandler<CancelItemCommand, SaleDto>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IEventSourcingHandler<SaleAggregate> _eventSourcingHandler;

    public CancelItemCommandHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        IEventSourcingHandler<SaleAggregate> eventSourcingHandler)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _eventSourcingHandler = eventSourcingHandler;
    }

    public async Task<SaleDto> Handle(CancelItemCommand request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(request.SaleId, cancellationToken)
                   ?? throw new KeyNotFoundException($"Sale with ID {request.SaleId} not found.");

        var item = sale.Items.FirstOrDefault(i => i.Id == request.ItemId)
                   ?? throw new KeyNotFoundException($"Item {request.ItemId} not found in sale {request.SaleId}.");

        if (item.IsActive())
        {
            var aggregateId = GetDeterministicGuid(sale.Id);
            var aggregate = await _eventSourcingHandler.GetByIdAsync(aggregateId);

            // If the stream does not exist, do not proceed
            if (aggregate.Version < 0)
                throw new InvalidOperationException(
                    $"No event stream found for aggregate {aggregateId}. Cannot cancel an item before creation.");

            // Ensure we are operating on the right stream
            if (aggregate.Id != aggregateId)
                throw new InvalidOperationException(
                    $"Aggregate mismatch. Expected {aggregateId}, got {aggregate.Id}.");

            sale.CancelItem(request.ItemId);
            aggregate.CancelItem(request.ItemId);

            await _eventSourcingHandler.SaveAsync(aggregate);
            await _saleRepository.UpdateAsync(sale, cancellationToken);
        }

        return _mapper.Map<SaleDto>(sale);
    }

    private static Guid GetDeterministicGuid(int saleId) =>
        Guid.Parse($"00000000-0000-0000-0000-{saleId:D12}");
}