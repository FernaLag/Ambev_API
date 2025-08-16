namespace Ambev.Ominia.Application.Features.Sales.Commands.CancelSale;

public record CancelSaleCommand(int Id) : IRequest<SaleDto>;