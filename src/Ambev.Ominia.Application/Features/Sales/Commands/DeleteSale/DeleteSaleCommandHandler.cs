namespace Ambev.Ominia.Application.Features.Sales.Commands.DeleteSale;

public class DeleteSaleCommandHandler(ISaleRepository saleRepository) : IRequestHandler<DeleteSaleCommand>
{
    public async Task Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
    {
        await saleRepository.DeleteAsync(request.Id, cancellationToken);
    }
}