using Ambev.Ominia.Application.Common;

namespace Ambev.Ominia.Application.Features.Carts.Queries.ListCarts;

public record ListCartsQuery : PaginatedListQuery, IRequest<List<CartSummaryDto>>
{
}