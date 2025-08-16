using Ambev.Ominia.Domain.Common;

namespace Ambev.Ominia.Domain.Events.Sales;

/// <summary>
/// Event triggered when a sale is cancelled.
/// </summary>
public class SaleCancelledEvent : BaseEvent
{
    /// <summary>
    /// Initializes a new instance of <see cref="SaleCancelledEvent"/>.
    /// </summary>
    public SaleCancelledEvent(Guid aggregateId)
        : base(nameof(SaleCancelledEvent))
    {
        Id = Guid.NewGuid();
        AggregateId = aggregateId;
    }
}