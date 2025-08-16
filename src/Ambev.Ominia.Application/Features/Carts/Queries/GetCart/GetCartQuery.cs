namespace Ambev.Ominia.Application.Features.Carts.Queries.GetCart;

/// <summary>
/// Command for retrieving a cart by its ID.
/// </summary>
public record GetCartQuery(int Id) : IRequest<CartDto>;