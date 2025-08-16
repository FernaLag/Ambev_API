namespace Ambev.Ominia.Domain.Interfaces;

/// <summary>
/// Interface for aggregates that have a status and can be marked as active or inactive.
/// </summary>
public interface IHasStatus
{
    /// <summary>
    /// Determines if the aggregate is active.
    /// </summary>
    /// <returns>True if the aggregate is active, otherwise false.</returns>
    bool IsActive();
}