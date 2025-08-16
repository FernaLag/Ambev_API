namespace Ambev.Ominia.Application.Features.Carts.Queries.GetCart;

public class GetCartQueryHandler(ICartRepository cartRepository, IMapper mapper) : IRequestHandler<GetCartQuery, CartDto>
{
    public async Task<CartDto> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var cart = await cartRepository.GetByIdAsync(request.Id, cancellationToken);
        return mapper.Map<CartDto>(cart);
    }
}