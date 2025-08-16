namespace Ambev.Ominia.Domain.Common;

public abstract class AggregateRoot
{
    protected Guid _id;
    private readonly List<BaseEvent> _changes = [];

    public Guid Id
    {
        get { return _id; }
    }

    public int Version { get; set; } = -1;

    public IEnumerable<BaseEvent> GetUncommittedChanges()
    {
        return _changes;
    }

    public void MarkChangesAsCommitted()
    {
        _changes.Clear();
    }

    private void ApplyChange(BaseEvent @event, bool isNew)
    {
        var method = GetType().GetMethod("Apply", [@event.GetType()]);

        if (method == null)
        {
            throw new ArgumentNullException(nameof(method), $"The Apply method was not found in the aggregate for {@event.GetType().Name}!");
        }

        method.Invoke(this, [@event]);

        if (isNew)
        {
            _changes.Add(@event);
        }
    }

    protected void RaiseEvent(BaseEvent @event)
    {
        // Only propagate the aggregate Id to the event if the aggregate already has one
        // AND the event didn't come with an explicit AggregateId.
        if (Id != Guid.Empty && @event.AggregateId == Guid.Empty)
            @event.AggregateId = Id;

        ApplyChange(@event, true);
    }

    public void ReplayEvents(IEnumerable<BaseEvent> events)
    {
        foreach (var @event in events)
        {
            ApplyChange(@event, false);
        }
    }
}