namespace Ambev.Ominia.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler(IProductRepository productRepository) : IRequestHandler<DeleteProductCommand>
{

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        await productRepository.DeleteAsync(request.Id, cancellationToken);
    }
}