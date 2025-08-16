namespace Ambev.Ominia.Application.Features.Products.Queries.GetProduct;

public class GetProductQueryHandler(IProductRepository productRepository, IMapper mapper) : IRequestHandler<GetProductQuery, ProductDto>
{

    public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken);

        return product is null
            ? throw new KeyNotFoundException($"Product with ID {request.Id} not found")
            : mapper.Map<ProductDto>(product);
    }
}