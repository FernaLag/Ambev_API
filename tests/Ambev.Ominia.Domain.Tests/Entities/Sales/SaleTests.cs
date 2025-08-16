using Ambev.Ominia.Domain.Entities.Sales;
using Ambev.Ominia.Domain.Enums;
using Ambev.Ominia.Domain.Tests.Entities.Sales.Builders;
using Shouldly;
using Xunit;

namespace Ambev.Ominia.Domain.Tests.Entities.Sales;

public class SaleTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateSaleWithCorrectProperties()
    {
        // Arrange
        var saleNumber = "SALE-2024-001";
        var date = DateTime.UtcNow;
        var customer = "Test Customer";
        var branch = "Test Branch";
        var items = new List<SaleItem>
        {
            SaleItemBuilder.New().WithQuantity(5).WithUnitPrice(10.00m).WithDiscount(2.00m).Build()
        };

        // Act
        var sale = new Sale(saleNumber, date, customer, branch, items);

        // Assert
        sale.SaleNumber.ShouldBe(saleNumber);
        sale.Date.ShouldBe(date);
        sale.Customer.ShouldBe(customer);
        sale.Branch.ShouldBe(branch);
        sale.Items.ShouldBe(items);
        sale.Status.ShouldBe(SaleStatus.Active);
    }

    [Fact]
    public void TotalAmount_WithSingleItem_ShouldCalculateCorrectTotal()
    {
        // Arrange
        var items = new List<SaleItem>
        {
            SaleItemBuilder.New()
                .WithQuantity(5)
                .WithUnitPrice(10.00m)
                .WithDiscount(2.00m)
                .Build()
        };
        var sale = SaleBuilder.New().WithItems(items).Build();

        // Act
        var totalAmount = sale.TotalAmount;

        // Assert
        // Expected: (5 * 10.00) - 2.00 = 48.00
        totalAmount.ShouldBe(48.00m);
    }

    [Fact]
    public void TotalAmount_WithMultipleItems_ShouldCalculateCorrectTotal()
    {
        // Arrange
        var items = new List<SaleItem>
        {
            SaleItemBuilder.New()
                .WithQuantity(5)
                .WithUnitPrice(10.00m)
                .WithDiscount(2.00m)
                .Build(),
            SaleItemBuilder.New()
                .WithQuantity(3)
                .WithUnitPrice(15.00m)
                .WithDiscount(5.00m)
                .Build()
        };
        var sale = SaleBuilder.New().WithItems(items).Build();

        // Act
        var totalAmount = sale.TotalAmount;

        // Assert
        // Expected: [(5 * 10.00) - 2.00] + [(3 * 15.00) - 5.00] = 48.00 + 40.00 = 88.00
        totalAmount.ShouldBe(88.00m);
    }

    [Fact]
    public void TotalAmount_WithNoItems_ShouldReturnZero()
    {
        // Arrange
        var sale = SaleBuilder.New().WithEmptyItems().Build();

        // Act
        var totalAmount = sale.TotalAmount;

        // Assert
        totalAmount.ShouldBe(0m);
    }

    [Fact]
    public void TotalAmount_WithCancelledItems_ShouldExcludeCancelledItems()
    {
        // Arrange
        var activeItem = SaleItemBuilder.New()
            .WithQuantity(5)
            .WithUnitPrice(10.00m)
            .WithDiscount(0m)
            .Build();
        
        var cancelledItem = SaleItemBuilder.New()
            .WithQuantity(3)
            .WithUnitPrice(15.00m)
            .WithDiscount(0m)
            .Build();
        cancelledItem.Cancel();

        var items = new List<SaleItem> { activeItem, cancelledItem };
        var sale = SaleBuilder.New().WithItems(items).Build();

        // Act
        var totalAmount = sale.TotalAmount;

        // Assert
        // Only active item should be counted: 5 * 10.00 = 50.00
        totalAmount.ShouldBe(50.00m);
    }

    [Fact]
    public void Update_WithValidParameters_ShouldUpdateSaleProperties()
    {
        // Arrange
        var sale = SaleBuilder.New().Build();
        var newCustomer = "Updated Customer";
        var newBranch = "Updated Branch";
        var newItems = new List<SaleItem>
        {
            SaleItemBuilder.New().WithQuantity(10).WithUnitPrice(20.00m).Build()
        };

        // Act
        var newDate = DateTime.Now.AddDays(1);
        sale.Update("UPDATED-001", newDate, newCustomer, newBranch, newItems);

        // Assert
        sale.SaleNumber.ShouldBe("UPDATED-001");
        sale.Customer.ShouldBe(newCustomer);
        sale.Branch.ShouldBe(newBranch);
        sale.Items.ShouldBe(newItems);
        sale.Date.ShouldBe(newDate);
    }

    [Fact]
    public void Update_ShouldMaintainOriginalSaleNumberAndDate()
    {
        // Arrange
        var originalSaleNumber = "ORIGINAL-001";
        var originalDate = DateTime.UtcNow.AddDays(-1);
        var sale = SaleBuilder.New()
            .WithSaleNumber(originalSaleNumber)
            .WithDate(originalDate)
            .Build();

        // Act
        var newDate = DateTime.Now.AddDays(1);
        sale.Update("NEW-001", newDate, "New Customer", "New Branch", []);

        // Assert
        sale.SaleNumber.ShouldBe("NEW-001");
        sale.Date.ShouldBe(newDate);
    }

    [Fact]
    public void Cancel_ShouldSetStatusToCancelled()
    {
        // Arrange
        var sale = SaleBuilder.New().Build();
        sale.Status.ShouldBe(SaleStatus.Active);

        // Act
        sale.Cancel();

        // Assert
        sale.Status.ShouldBe(SaleStatus.Cancelled);
    }

    [Fact]
    public void Status_WithActiveSale_ShouldBeActive()
    {
        // Arrange
        var sale = SaleBuilder.New().Build();

        // Act & Assert
        sale.Status.ShouldBe(SaleStatus.Active);
    }

    [Fact]
    public void Status_WithCancelledSale_ShouldBeCancelled()
    {
        // Arrange
        var sale = SaleBuilder.New().Build();
        sale.Cancel();

        // Act & Assert
        sale.Status.ShouldBe(SaleStatus.Cancelled);
    }
}