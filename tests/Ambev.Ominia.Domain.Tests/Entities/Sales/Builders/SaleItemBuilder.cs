using Ambev.Ominia.Domain.Entities.Sales;
using Ambev.Ominia.Domain.Entities.Products;

namespace Ambev.Ominia.Domain.Tests.Entities.Sales.Builders;

public class SaleItemBuilder
{
    private int _saleId = 1;
    private int _productId = 1;
    private int _quantity = 5;
    private decimal _unitPrice = 10.00m;
    private decimal _discount = 0m;
    private Product? _product;

    public static SaleItemBuilder New() => new();

    public SaleItemBuilder WithSaleId(int saleId)
    {
        _saleId = saleId;
        return this;
    }

    public SaleItemBuilder WithProductId(int productId)
    {
        _productId = productId;
        return this;
    }

    public SaleItemBuilder WithQuantity(int quantity)
    {
        _quantity = quantity;
        return this;
    }

    public SaleItemBuilder WithUnitPrice(decimal unitPrice)
    {
        _unitPrice = unitPrice;
        return this;
    }

    public SaleItemBuilder WithDiscount(decimal discount)
    {
        _discount = discount;
        return this;
    }

    public SaleItemBuilder WithProduct(Product? product)
    {
        _product = product;
        return this;
    }

    public SaleItemBuilder WithQuantityBelowDiscountLimit()
    {
        _quantity = 3;
        _discount = 5.00m;
        return this;
    }

    public SaleItemBuilder WithQuantityAboveLimit()
    {
        _quantity = 25;
        return this;
    }

    public SaleItemBuilder WithValidQuantityAndDiscount()
    {
        _quantity = 4;
        _discount = 2.00m;
        return this;
    }

    public SaleItem Build()
    {
        var item = new SaleItem(_saleId, _productId, _quantity, _unitPrice, _discount);
        if (_product != null)
        {
            item.Product = _product;
        }
        return item;
    }
}