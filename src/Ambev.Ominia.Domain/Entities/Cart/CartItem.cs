using Ambev.Ominia.Domain.Common;
using Ambev.Ominia.Domain.Entities.Products;

namespace Ambev.Ominia.Domain.Entities.Cart;

public class CartItem : BaseEntity
{
    public int CartId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; } = 1;

    public Cart? Cart { get; set; }

    public Product? Product { get; set; }
}