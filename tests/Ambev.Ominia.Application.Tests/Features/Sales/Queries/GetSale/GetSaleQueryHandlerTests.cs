using Ambev.Ominia.Application.Features.Sales;
using Ambev.Ominia.Application.Features.Sales.Queries.GetSale;
using Ambev.Ominia.Application.Tests.Features.Sales.Builders;
using Ambev.Ominia.Domain.Entities.Sales;
using Ambev.Ominia.Domain.Interfaces.Infrastructure;
using AutoMapper;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Ambev.Ominia.Application.Tests.Features.Sales.Queries.GetSale;

public class GetSaleQueryHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly GetSaleQueryHandler _handler;

    public GetSaleQueryHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetSaleQueryHandler(_saleRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ValidQuery_ShouldReturnSaleDto()
    {
        // Arrange
        var query = new GetSaleQueryBuilder().WithId(1).Build();
        var sale = new SaleBuilder().Build();
        var expectedDto = new SaleDto { Id = 1, SaleNumber = sale.SaleNumber };

        // Simulate repository setting the ID
        typeof(Sale).GetProperty("Id")?.SetValue(sale, 1);

        _saleRepository.GetByIdAsync(1, Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<SaleDto>(sale).Returns(expectedDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(1);
        result.SaleNumber.ShouldBe(sale.SaleNumber);
        
        await _saleRepository.Received(1).GetByIdAsync(1, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<SaleDto>(sale);
    }

    [Fact]
    public async Task Handle_ValidQuery_ShouldCallRepositoryWithCorrectId()
    {
        // Arrange
        var saleId = 123;
        var query = new GetSaleQueryBuilder().WithId(saleId).Build();
        var sale = new SaleBuilder().Build();
        var expectedDto = new SaleDto();

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<SaleDto>(sale).Returns(expectedDto);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        await _saleRepository.Received(1).GetByIdAsync(saleId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ValidQuery_ShouldPassCancellationToken()
    {
        // Arrange
        var query = new GetSaleQueryBuilder().Build();
        var sale = new SaleBuilder().Build();
        var expectedDto = new SaleDto();
        var cancellationToken = new CancellationToken();

        _saleRepository.GetByIdAsync(query.Id, cancellationToken).Returns(sale);
        _mapper.Map<SaleDto>(sale).Returns(expectedDto);

        // Act
        await _handler.Handle(query, cancellationToken);

        // Assert
        await _saleRepository.Received(1).GetByIdAsync(query.Id, cancellationToken);
    }

    [Fact]
    public async Task Handle_ValidQuery_ShouldMapSaleToDto()
    {
        // Arrange
        var query = new GetSaleQueryBuilder().Build();
        var sale = new SaleBuilder().Build();
        var expectedDto = new SaleDto();

        _saleRepository.GetByIdAsync(query.Id, Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<SaleDto>(sale).Returns(expectedDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldBe(expectedDto);
        _mapper.Received(1).Map<SaleDto>(sale);
    }
}