namespace Ambev.Ominia.Application.Features.Carts.Queries.ListCarts;

public class ListCartsQueryHandler(ICartRepository cartRepository, IMapper mapper) : IRequestHandler<ListCartsQuery, List<CartSummaryDto>>
{
    public async Task<List<CartSummaryDto>> Handle(ListCartsQuery request, CancellationToken cancellationToken)
    {
        var carts = await cartRepository.ListAsync(request.Page, request.PageSize, request.OrderBy, cancellationToken);
        return mapper.Map<List<CartSummaryDto>>(carts);
    }
}