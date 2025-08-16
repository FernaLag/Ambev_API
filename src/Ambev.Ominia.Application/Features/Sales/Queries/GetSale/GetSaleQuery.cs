namespace Ambev.Ominia.Application.Features.Sales.Queries.GetSale;

public record GetSaleQuery(int Id) : IRequest<SaleDto>;