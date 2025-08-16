using Ambev.Ominia.Application.Common;

namespace Ambev.Ominia.Application.Features.Products.Queries.ListProducts;

public record ListProductsQuery : PaginatedListQuery, IRequest<List<ProductSummaryDto>>
{
    public string Category { get; set; } = string.Empty;

    public ListProductsQuery()
    {
    }

    public ListProductsQuery(string category)
    {
        Category = category;
    }
}