using Ambev.Ominia.Domain.Entities.Sales;

namespace Ambev.Ominia.Application.Features.Sales.Commands.CreateSale;

public class CreateSaleCommand : IRequest<SaleDto>
{
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Customer { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;
    public List<SaleItem> Items { get; set; } = [];
}