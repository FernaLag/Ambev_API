using Ambev.Ominia.Domain.Common;
using Ambev.Ominia.Domain.Entities.Cart;
using Ambev.Ominia.Domain.ValueObjects;

namespace Ambev.Ominia.Domain.Entities.Products;

/// <summary>
/// Represents a product available for sale in the system.
/// </summary>
public class Product : BaseEntity
{
    /// <summary>
    /// Gets or sets the title of the product.
    /// Must not be null or empty.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the price of the product.
    /// Must be greater than zero.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the description of the product.
    /// Provides additional details about the product.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the category to which the product belongs.
    /// Helps in organizing and filtering products.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the image URL of the product.
    /// Must be a valid URL.
    /// </summary>
    public string Image { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the rating details of the product.
    /// Includes both rating value and number of votes.
    /// </summary>
    public Rating Rating { get; set; } = new(0, 0);

    /// <summary>
    /// Gets the creation timestamp of the product.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the last update timestamp of the product.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the collection of products in the cart.
    /// </summary>
    public List<CartItem> CartItems { get; set; } = [];
}