using Ambev.Ominia.Domain.Entities.Products;

namespace Ambev.Ominia.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler(IProductRepository productRepository, IMapper mapper) : IRequestHandler<CreateProductCommand, ProductDto>
{

    public async Task<ProductDto> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = mapper.Map<Product>(command);
        var createdProduct = await productRepository.CreateAsync(product, cancellationToken);
        var result = mapper.Map<ProductDto>(createdProduct);
        return result;
    }
}