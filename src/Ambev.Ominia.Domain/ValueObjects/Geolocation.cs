namespace Ambev.Ominia.Domain.ValueObjects;

/// <summary>
/// Represents a geographical location with latitude and longitude coordinates.
/// Used in the <see cref="Address"/> value object to specify the exact location of a user.
/// </summary>
public class Geolocation
{
    /// <summary>
    /// The latitude coordinate.
    /// </summary>
    public string Latitude { get; set; } = string.Empty;
    
    /// <summary>
    /// The longitude coordinate.
    /// </summary>
    public string Longitude { get; set; } = string.Empty;
    
    /// <summary>
    /// Parameterless constructor for EF Core.
    /// </summary>
    public Geolocation() { }
}