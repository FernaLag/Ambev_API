namespace Ambev.Ominia.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler(IProductRepository productRepository, IMapper mapper) : IRequestHandler<UpdateProductCommand, ProductDto>
{

    public async Task<ProductDto> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(command.Id, cancellationToken);

        mapper.Map(command, product);
        var updatedProduct = await productRepository.UpdateAsync(product, cancellationToken);

        return mapper.Map<ProductDto>(updatedProduct);
    }
}