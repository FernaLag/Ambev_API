namespace Ambev.Ominia.Application.Features.Carts.Commands.UpdateCart;

public class UpdateCartCommandHandler(ICartRepository cartRepository, IMapper mapper) : IRequestHandler<UpdateCartCommand, CartDto>
{
    public async Task<CartDto> Handle(UpdateCartCommand command, CancellationToken cancellationToken)
    {
        var cart = await cartRepository.GetByIdAsync(command.Id, cancellationToken);

        cart.Update(command.UserId, command.Date, command.CartItems);

        await cartRepository.UpdateAsync(cart, cancellationToken);

        return mapper.Map<CartDto>(cart);
    }
}