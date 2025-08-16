using Ambev.Ominia.Domain.Entities.Cart;

namespace Ambev.Ominia.Application.Features.Carts.Commands.CreateCart;

public class CreateCartCommandHandler(ICartRepository cartRepository, IMapper mapper) : IRequestHandler<CreateCartCommand, CartDto>
{
    public async Task<CartDto> Handle(CreateCartCommand command, CancellationToken cancellationToken)
    {
        var cart = new Cart
        {
            UserId = command.UserId,
            Date = command.Date,
            CartItems = []
        };

        var createdCart = await cartRepository.CreateAsync(cart, cancellationToken);

        createdCart.CartItems = command.Products.Select(p => new CartItem
        {
            CartId = createdCart.Id,
            ProductId = p.ProductId,
            Quantity = p.Quantity
        }).ToList();

        await cartRepository.UpdateAsync(createdCart, cancellationToken);

        var result = mapper.Map<CartDto>(createdCart);
        return result;
    }
}