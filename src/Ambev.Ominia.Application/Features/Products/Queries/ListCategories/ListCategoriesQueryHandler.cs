namespace Ambev.Ominia.Application.Features.Products.Queries.ListCategories;

public class ListCategoriesQueryHandler(IProductRepository productRepository) : IRequestHandler<ListCategoriesQuery, IEnumerable<string>>
{

    public async Task<IEnumerable<string>> Handle(ListCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await productRepository.GetCategoriesAsync(cancellationToken);
    }
}