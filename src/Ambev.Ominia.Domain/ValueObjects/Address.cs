namespace Ambev.Ominia.Domain.ValueObjects;

/// <summary>
/// Represents a user's address, containing city, street, house/building number, ZIP code, and geolocation.
/// This value object is used in the <see cref="User"/> entity.
/// </summary>
public class Address
{
    /// <summary>
    /// The city where the user resides.
    /// </summary>
    public string City { get; set; } = string.Empty;
    
    /// <summary>
    /// The street name of the user's address.
    /// </summary>
    public string Street { get; set; } = string.Empty;
    
    /// <summary>
    /// The house or building number.
    /// </summary>
    public int Number { get; set; }
    
    /// <summary>
    /// The ZIP code for postal identification.
    /// </summary>
    public string ZipCode { get; set; } = string.Empty;
    
    /// <summary>
    /// The geolocation coordinates of the address.
    /// </summary>
    public Geolocation Geolocation { get; set; } = new();
    
    /// <summary>
    /// Parameterless constructor for EF Core.
    /// </summary>
    public Address()
    {
    }
}