using Ambev.Ominia.Domain.Entities.Cart;

namespace Ambev.Ominia.Application.Features.Carts.Commands.UpdateCart;

/// <summary>
/// Command for updating an existing cart.
/// </summary>
public class UpdateCartCommand : IRequest<CartDto>
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public List<CartItem> CartItems { get; set; } = [];
}