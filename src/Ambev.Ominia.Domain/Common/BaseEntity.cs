namespace Ambev.Ominia.Domain.Common;

/// <summary>
/// Base entity for all domain entities.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Unique identifier for the entity.
    /// </summary>
    public int Id { get; set; }
}