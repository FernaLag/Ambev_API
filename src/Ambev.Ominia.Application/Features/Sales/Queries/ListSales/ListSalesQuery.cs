using Ambev.Ominia.Application.Common;

namespace Ambev.Ominia.Application.Features.Sales.Queries.ListSales;

public record ListSalesQuery : PaginatedListQuery, IRequest<List<SaleSummaryDto>>
{
}