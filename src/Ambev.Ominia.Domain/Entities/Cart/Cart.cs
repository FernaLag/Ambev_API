using Ambev.Ominia.Domain.Common;

namespace Ambev.Ominia.Domain.Entities.Cart;

/// <summary>
/// Represents a shopping cart in the system.
/// </summary>
public class Cart : BaseEntity
{
    /// <summary>
    /// Gets or sets the user ID associated with the cart.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the date when the cart was created or last updated.
    /// </summary>
    public DateTime Date { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the collection of items in the cart.
    /// </summary>
    public List<CartItem> CartItems { get; set; } = [];

    /// <summary>
    /// Updates the cart details with new values.
    /// </summary>
    /// <param name="userId">New user ID.</param>
    /// <param name="date">New date.</param>
    /// <param name="cartItems">New list of cart items.</param>
    public void Update(int userId, DateTime date, List<CartItem> cartItems)
    {
        UserId = userId;
        Date = date;
        CartItems = cartItems;
    }
}