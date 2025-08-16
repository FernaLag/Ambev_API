using Ambev.Ominia.Domain.Entities.Sales;
using Bogus;

namespace Ambev.Ominia.Application.Tests.Features.Sales.Builders;

public class SaleItemBuilder
{
    private int _saleId = 0;
    private int _productId = 1;
    private int _quantity = 1;
    private decimal _unitPrice = 10.00m;
    private decimal _discount = 0m;
    private readonly Faker _faker = new();

    public SaleItemBuilder()
    {
        WithDefaults();
    }

    public SaleItemBuilder WithDefaults()
    {
        _saleId = _faker.Random.Int(1, 1000);
        _productId = _faker.Random.Int(1, 100);
        _quantity = _faker.Random.Int(1, 5);
        _unitPrice = _faker.Random.Decimal(10, 1000);
        _discount = 0m;
        return this;
    }

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

    public SaleItemBuilder WithQuantityForDiscount10Percent()
    {
        _quantity = _faker.Random.Int(4, 9);
        return this;
    }

    public SaleItemBuilder WithQuantityForDiscount20Percent()
    {
        _quantity = _faker.Random.Int(10, 20);
        return this;
    }

    public SaleItemBuilder WithQuantityAboveLimit()
    {
        _quantity = _faker.Random.Int(21, 50);
        return this;
    }

    public SaleItemBuilder WithQuantityBelowDiscountThreshold()
    {
        _quantity = _faker.Random.Int(1, 3);
        return this;
    }

    public SaleItem Build()
    {
        return new SaleItem(_saleId, _productId, _quantity, _unitPrice, _discount);
    }
}