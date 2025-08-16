namespace Ambev.Ominia.Application.Features.Sales.Queries.GetSale;

public class GetSaleQueryHandler(ISaleRepository saleRepository, IMapper mapper) : IRequestHandler<GetSaleQuery, SaleDto>
{
    public async Task<SaleDto> Handle(GetSaleQuery request, CancellationToken cancellationToken)
    {
        var sale = await saleRepository.GetByIdAsync(request.Id, cancellationToken);

        return sale is null
            ? throw new KeyNotFoundException($"Sale with ID {request.Id} not found")
            : mapper.Map<SaleDto>(sale);
    }
}