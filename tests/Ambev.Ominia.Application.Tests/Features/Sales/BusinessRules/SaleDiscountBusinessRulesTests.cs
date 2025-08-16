using Ambev.Ominia.Application.Tests.Features.Sales.Builders;
using Ambev.Ominia.Domain.Services;
using Shouldly;
using Xunit;

namespace Ambev.Ominia.Application.Tests.Features.Sales.BusinessRules;

public class SaleDiscountBusinessRulesTests
{
    [Theory]
    [InlineData(1, 0)]
    [InlineData(2, 0)]
    [InlineData(3, 0)]
    public void CalculateDiscount_LessThan4Items_ShouldNotReceiveDiscount(int quantity, decimal expectedDiscountPercentage)
    {
        // Arrange
        var saleItem = new SaleItemBuilder()
            .WithQuantity(quantity)
            .WithUnitPrice(10.00m)
            .Build();

        // Act
        var discountPercentage = SaleDiscountService.CalculateDiscountPercentage(saleItem.Quantity);
        var totalPrice = saleItem.Quantity * saleItem.UnitPrice;
        var discountAmount = totalPrice * (discountPercentage / 100);
        var finalPrice = totalPrice - discountAmount;

        // Assert
        discountPercentage.ShouldBe(expectedDiscountPercentage);
        finalPrice.ShouldBe(totalPrice);
    }

    [Theory]
    [InlineData(4, 10)]
    [InlineData(5, 10)]
    [InlineData(6, 10)]
    [InlineData(7, 10)]
    [InlineData(8, 10)]
    [InlineData(9, 10)]
    public void CalculateDiscount_4To9Items_ShouldReceive10PercentDiscount(int quantity, decimal expectedDiscountPercentage)
    {
        // Arrange
        var saleItem = new SaleItemBuilder()
            .WithQuantity(quantity)
            .WithUnitPrice(10.00m)
            .Build();

        // Act
        var discountPercentage = SaleDiscountService.CalculateDiscountPercentage(saleItem.Quantity);
        var totalPrice = saleItem.Quantity * saleItem.UnitPrice;
        var discountAmount = totalPrice * (discountPercentage / 100);
        var finalPrice = totalPrice - discountAmount;

        // Assert
        discountPercentage.ShouldBe(expectedDiscountPercentage);
        finalPrice.ShouldBe(totalPrice * 0.9m);
    }

    [Theory]
    [InlineData(10, 20)]
    [InlineData(11, 20)]
    [InlineData(15, 20)]
    [InlineData(18, 20)]
    [InlineData(20, 20)]
    public void CalculateDiscount_10To20Items_ShouldReceive20PercentDiscount(int quantity, decimal expectedDiscountPercentage)
    {
        // Arrange
        var saleItem = new SaleItemBuilder()
            .WithQuantity(quantity)
            .WithUnitPrice(10.00m)
            .Build();

        // Act
        var discountPercentage = SaleDiscountService.CalculateDiscountPercentage(saleItem.Quantity);
        var totalPrice = saleItem.Quantity * saleItem.UnitPrice;
        var discountAmount = totalPrice * (discountPercentage / 100);
        var finalPrice = totalPrice - discountAmount;

        // Assert
        discountPercentage.ShouldBe(expectedDiscountPercentage);
        finalPrice.ShouldBe(totalPrice * 0.8m);
    }

    [Theory]
    [InlineData(21)]
    [InlineData(25)]
    [InlineData(100)]
    public void ValidateQuantity_MoreThan20Items_ShouldThrowException(int quantity)
    {
        // Arrange & Act & Assert
        var exception = Should.Throw<InvalidOperationException>(() => SaleDiscountService.ValidateMaxQuantity(quantity));
        exception.Message.ShouldContain("Cannot sell more than 20 identical items");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(20)]
    public void ValidateQuantity_20OrLessItems_ShouldNotThrowException(int quantity)
    {
        // Arrange & Act & Assert
        Should.NotThrow(() => SaleDiscountService.ValidateMaxQuantity(quantity));
    }

    [Fact]
    public void CalculateDiscount_MultipleItemsWithDifferentQuantities_ShouldApplyCorrectDiscounts()
    {
        // Arrange
        var sale = new SaleBuilder()
            .WithItems([
                new SaleItemBuilder().WithQuantity(3).WithUnitPrice(10.00m).Build(), // No discount
                new SaleItemBuilder().WithQuantity(5).WithUnitPrice(20.00m).Build(), // 10% discount
                new SaleItemBuilder().WithQuantity(15).WithUnitPrice(30.00m).Build()
                ])
            .Build();

        // Act
        var item1Total = 3 * 10.00m; // 30.00 (no discount)
        var item2Total = 5 * 20.00m * 0.9m; // 90.00 (10% discount)
        var item3Total = 15 * 30.00m * 0.8m; // 360.00 (20% discount)
        var expectedTotal = item1Total + item2Total + item3Total;

        // Assert
        var actualTotal = SaleDiscountService.CalculateSaleTotal(sale);
        actualTotal.ShouldBe(expectedTotal);
    }

    [Fact]
    public void CalculateDiscount_ExactBoundaryValues_ShouldApplyCorrectDiscounts()
    {
        // Arrange & Act & Assert
        SaleDiscountService.CalculateDiscountPercentage(3).ShouldBe(0); // Just below 4
        SaleDiscountService.CalculateDiscountPercentage(4).ShouldBe(10); // Exactly 4
        SaleDiscountService.CalculateDiscountPercentage(9).ShouldBe(10); // Just below 10
        SaleDiscountService.CalculateDiscountPercentage(10).ShouldBe(20); // Exactly 10
        SaleDiscountService.CalculateDiscountPercentage(20).ShouldBe(20); // Exactly 20
    }


}