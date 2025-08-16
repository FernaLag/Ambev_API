using Ambev.Ominia.Application.Features.Sales;
using Ambev.Ominia.Application.Features.Sales.Queries.ListSales;
using Ambev.Ominia.Application.Tests.Features.Sales.Builders;
using Ambev.Ominia.Domain.Entities.Sales;
using Ambev.Ominia.Domain.Interfaces.Infrastructure;
using AutoMapper;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Ambev.Ominia.Application.Tests.Features.Sales.Queries.ListSales;

public class ListSalesQueryHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ListSalesQueryHandler _handler;

    public ListSalesQueryHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new ListSalesQueryHandler(_saleRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ValidQuery_ShouldReturnListOfSaleDtos()
    {
        // Arrange
        var query = new ListSalesQueryBuilder().Build();
        var sales = new List<Sale>
        {
            new SaleBuilder().Build(),
            new SaleBuilder().Build()
        };
        var expectedDtos = new List<SaleSummaryDto>
        {
            new SaleSummaryDto { Id = 1, SaleNumber = "SALE-001" },
            new SaleSummaryDto { Id = 2, SaleNumber = "SALE-002" }
        };

        _saleRepository.ListAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(sales);
        _mapper.Map<List<SaleSummaryDto>>(sales).Returns(expectedDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Count().ShouldBe(2);
        result.ShouldBe(expectedDtos);
        
        await _saleRepository.Received(1).ListAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<List<SaleSummaryDto>>(sales);
    }

    [Fact]
    public async Task Handle_ValidQuery_ShouldReturnEmptyListWhenNoSales()
    {
        // Arrange
        var query = new ListSalesQueryBuilder().Build();
        var sales = new List<Sale>();
        var expectedDtos = new List<SaleSummaryDto>();

        _saleRepository.ListAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(sales);
        _mapper.Map<List<SaleSummaryDto>>(sales).Returns(expectedDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Count().ShouldBe(0);
        result.ShouldBe(expectedDtos);
    }

    [Fact]
    public async Task Handle_ValidQuery_ShouldPassCancellationToken()
    {
        // Arrange
        var query = new ListSalesQueryBuilder().Build();
        var sales = new List<Sale>();
        var expectedDtos = new List<SaleSummaryDto>();
        var cancellationToken = new CancellationToken();

        _saleRepository.ListAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(), cancellationToken).Returns(sales);
        _mapper.Map<List<SaleSummaryDto>>(sales).Returns(expectedDtos);

        // Act
        await _handler.Handle(query, cancellationToken);

        // Assert
        await _saleRepository.Received(1).ListAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(), cancellationToken);
    }

    [Fact]
    public async Task Handle_ValidQuery_ShouldMapSalesToDtos()
    {
        // Arrange
        var query = new ListSalesQueryBuilder().Build();
        var sales = new List<Sale> { new SaleBuilder().Build() };
        var expectedDtos = new List<SaleSummaryDto> { new SaleSummaryDto() };

        _saleRepository.ListAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(sales);
        _mapper.Map<List<SaleSummaryDto>>(sales).Returns(expectedDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldBe(expectedDtos);
        _mapper.Received(1).Map<List<SaleSummaryDto>>(sales);
    }
}