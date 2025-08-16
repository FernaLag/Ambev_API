namespace Ambev.Ominia.Application.Features.Sales;

public record SaleDto
{
    public int Id { get; init; }
    public string SaleNumber { get; init; } = string.Empty;
    public DateTime Date { get; init; }
    public string CustomerName { get; init; } = string.Empty;
    public string BranchName { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public IReadOnlyList<SaleItemDto> Items { get; init; } = new List<SaleItemDto>();
    
    /// <summary>
    /// Parameterless constructor for AutoMapper.
    /// </summary>
    public SaleDto() { }
    
    /// <summary>
    /// Constructor with parameters.
    /// </summary>
    public SaleDto(int id, string saleNumber, DateTime date, string customerName, string branchName, string status, IReadOnlyList<SaleItemDto> items)
    {
        Id = id;
        SaleNumber = saleNumber;
        Date = date;
        CustomerName = customerName;
        BranchName = branchName;
        Status = status;
        Items = items;
    }
}

public record SaleItemDto
{
    public int Id { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal Total { get; init; }
    public string Status { get; init; } = string.Empty;
    
    /// <summary>
    /// Parameterless constructor for AutoMapper.
    /// </summary>
    public SaleItemDto() { }
    
    /// <summary>
    /// Constructor with parameters.
    /// </summary>
    public SaleItemDto(int id, string productName, int quantity, decimal unitPrice, decimal total, string status)
    {
        Id = id;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Total = total;
        Status = status;
    }
}

public record SaleSummaryDto
{
    public int Id { get; init; }
    public string SaleNumber { get; init; } = string.Empty;
    public DateTime Date { get; init; }
    public string CustomerName { get; init; } = string.Empty;
    public string BranchName { get; init; } = string.Empty;
    
    /// <summary>
    /// Parameterless constructor for AutoMapper.
    /// </summary>
    public SaleSummaryDto() { }
    
    /// <summary>
    /// Constructor with parameters.
    /// </summary>
    public SaleSummaryDto(int id, string saleNumber, DateTime date, string customerName, string branchName)
    {
        Id = id;
        SaleNumber = saleNumber;
        Date = date;
        CustomerName = customerName;
        BranchName = branchName;
    }
}