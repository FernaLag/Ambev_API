namespace Ambev.Ominia.Application.Features.Sales.Queries.ListSales;

public class ListSalesQueryHandler(ISaleRepository saleRepository, IMapper mapper) : IRequestHandler<ListSalesQuery, List<SaleSummaryDto>>
{
    public async Task<List<SaleSummaryDto>> Handle(ListSalesQuery request, CancellationToken cancellationToken)
    {
        var sales = await saleRepository.ListAsync(request.Page, request.PageSize, request.OrderBy, cancellationToken);
        return mapper.Map<List<SaleSummaryDto>>(sales);
    }
}