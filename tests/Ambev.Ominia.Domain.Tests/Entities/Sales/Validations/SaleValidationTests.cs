using System.Reflection;
using Ambev.Ominia.Domain.Entities.Sales;
using Ambev.Ominia.Domain.Entities.Sales.Validations;
using Ambev.Ominia.Domain.Tests.Entities.Sales.Builders;
using Shouldly;
using Xunit;

namespace Ambev.Ominia.Domain.Tests.Entities.Sales.Validations;

public class SaleValidationTests
{
    [Fact]
    public void CanCreateSale_WithValidSale_ShouldReturnNoFailures()
    {
        var sale = SaleBuilder.New()
            .WithValidItems()
            .Build();

        var result = SaleValidation.CanCreateSale(sale);

        result.ShouldBeEmpty();
    }

    [Fact]
    public void CanCreateSale_WithNullSaleNumber_ShouldReturnValidationFailure()
    {
        var sale = SaleBuilder.New()
            .WithValidItems()
            .Build();
        
        // Use reflection to set invalid sale number for validation testing
        var saleNumberProperty = typeof(Sale).GetProperty("SaleNumber", BindingFlags.Public | BindingFlags.Instance);
        saleNumberProperty?.SetValue(sale, null);

        var result = SaleValidation.CanCreateSale(sale);

        result.ShouldNotBeEmpty();
        result.ShouldContain(f => f.PropertyName == "SaleNumber");
    }

    [Fact]
    public void CanCreateSale_WithNullCustomer_ShouldReturnValidationFailure()
    {
        var sale = SaleBuilder.New()
            .WithValidItems()
            .Build();
        
        // Use reflection to set invalid customer for validation testing
        var customerProperty = typeof(Sale).GetProperty("Customer", BindingFlags.Public | BindingFlags.Instance);
        customerProperty?.SetValue(sale, null);

        var result = SaleValidation.CanCreateSale(sale);

        result.ShouldNotBeEmpty();
        result.ShouldContain(f => f.PropertyName == "Customer");
    }

    [Fact]
    public void CanCreateSale_WithNullBranch_ShouldReturnValidationFailure()
    {
        var sale = SaleBuilder.New()
            .WithValidItems()
            .Build();
        
        // Use reflection to set invalid branch for validation testing
        var branchProperty = typeof(Sale).GetProperty("Branch", BindingFlags.Public | BindingFlags.Instance);
        branchProperty?.SetValue(sale, null);

        var result = SaleValidation.CanCreateSale(sale);

        result.ShouldNotBeEmpty();
        result.ShouldContain(f => f.PropertyName == "Branch");
    }

    [Fact]
    public void CanCreateSale_WithEmptyItems_ShouldReturnValidationFailure()
    {
        var sale = SaleBuilder.New()
            .WithEmptyItems()
            .Build();

        var result = SaleValidation.CanCreateSale(sale);

        result.ShouldNotBeEmpty();
        result.ShouldContain(f => f.PropertyName == "Items");
    }

    [Fact]
    public void CanCreateSale_WithItemQuantityBelowDiscountLimit_ShouldReturnValidationFailure()
    {
        var items = new List<Ambev.Ominia.Domain.Entities.Sales.SaleItem> { SaleItemBuilder.New().WithQuantityBelowDiscountLimit().Build() };
        var sale = SaleBuilder.New()
            .WithItems(items)
            .Build();

        var result = SaleValidation.CanCreateSale(sale);

        result.ShouldNotBeEmpty();
        result.ShouldContain(f => f.PropertyName == "Quantity" && f.ErrorMessage == "Discount is not allowed for purchases below 4 items.");
    }

    [Fact]
    public void CanCreateSale_WithItemQuantityAboveLimit_ShouldReturnValidationFailure()
    {
        var items = new List<Ambev.Ominia.Domain.Entities.Sales.SaleItem> { SaleItemBuilder.New().WithQuantityAboveLimit().Build() };
        var sale = SaleBuilder.New()
            .WithItems(items)
            .Build();

        var result = SaleValidation.CanCreateSale(sale);

        result.ShouldNotBeEmpty();
        result.ShouldContain(f => f.PropertyName == "Quantity" && f.ErrorMessage == "Cannot sell more than 20 identical items.");
    }

    [Fact]
    public void CanCreateSale_WithValidQuantityAndDiscount_ShouldReturnNoFailures()
    {
        var items = new List<Ambev.Ominia.Domain.Entities.Sales.SaleItem> { SaleItemBuilder.New().WithValidQuantityAndDiscount().Build() };
        var sale = SaleBuilder.New()
            .WithItems(items)
            .Build();

        var result = SaleValidation.CanCreateSale(sale);

        result.ShouldBeEmpty();
    }

    [Fact]
    public void CanCreateSale_WithMultipleValidationFailures_ShouldReturnAllFailures()
    {
        var items = new List<Ambev.Ominia.Domain.Entities.Sales.SaleItem>
         {
             SaleItemBuilder.New().WithQuantityBelowDiscountLimit().Build(),
             SaleItemBuilder.New().WithQuantityAboveLimit().Build()
         };
        
        // Create a sale with valid data first, then modify it to have invalid data for validation testing
        var sale = SaleBuilder.New()
            .WithItems(items)
            .Build();
        
        // Use reflection to set invalid customer for validation testing
        var customerProperty = typeof(Sale).GetProperty("Customer", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        customerProperty?.SetValue(sale, null);

        var result = SaleValidation.CanCreateSale(sale);

        result.Count.ShouldBe(3);
        result.ShouldContain(f => f.PropertyName == "Customer");
        result.ShouldContain(f => f.PropertyName == "Quantity" && f.ErrorMessage == "Discount is not allowed for purchases below 4 items.");
        result.ShouldContain(f => f.PropertyName == "Quantity" && f.ErrorMessage == "Cannot sell more than 20 identical items.");
    }
}