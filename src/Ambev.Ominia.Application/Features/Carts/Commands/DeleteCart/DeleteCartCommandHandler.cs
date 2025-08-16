namespace Ambev.Ominia.Application.Features.Carts.Commands.DeleteCart;

public class DeleteCartCommandHandler(ICartRepository cartRepository) : IRequestHandler<DeleteCartCommand>
{
    public async Task Handle(DeleteCartCommand request, CancellationToken cancellationToken)
    {
        await cartRepository.DeleteAsync(request.Id, cancellationToken);
    }
}