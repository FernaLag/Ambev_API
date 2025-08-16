namespace Ambev.Ominia.Application.Features.Carts.Commands.DeleteCart;

/// <summary>
/// Command for deleting a cart.
/// </summary>
public record DeleteCartCommand(int Id) : IRequest;