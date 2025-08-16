namespace Ambev.Ominia.Application.Features.Products.Queries.ListProducts;

public class ListProductsQueryHandler(IProductRepository productRepository, IMapper mapper) : IRequestHandler<ListProductsQuery, List<ProductSummaryDto>>
{
    public async Task<List<ProductSummaryDto>> Handle(ListProductsQuery request, CancellationToken cancellationToken)
    {
        List<Domain.Entities.Products.Product> products;
        
        if (!string.IsNullOrEmpty(request.Category))
        {
            products = await productRepository.ListByCategoryAsync(request.Category, request.Page, request.PageSize, request.OrderBy, cancellationToken);
        }
        else
        {
            products = await productRepository.ListAsync(request.Page, request.PageSize, request.OrderBy, cancellationToken);
        }
        
        return mapper.Map<List<ProductSummaryDto>>(products);
    }
}