using Ambev.Ominia.Domain.Entities.Sales;

namespace Ambev.Ominia.Application.Features.Sales.Commands.UpdateSale;

public class UpdateSaleCommand : IRequest<SaleDto>
{
    public int Id { get; set; }
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Customer { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;
    public List<SaleItem> Items { get; set; } = [];
}