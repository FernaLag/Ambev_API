using Ambev.Ominia.Application.Features.Sales;
using Ambev.Ominia.Application.Features.Sales.Commands.CreateSale;
using Ambev.Ominia.Application.Tests.Features.Sales.Builders;
using Ambev.Ominia.Domain.Aggregates;
using Ambev.Ominia.Domain.Entities.Sales;
using Ambev.Ominia.Domain.Events.Sales;
using Ambev.Ominia.Domain.Handlers;
using Ambev.Ominia.Domain.Interfaces.Infrastructure;
using AutoMapper;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Ambev.Ominia.Application.Tests.Features.Sales.Commands.CreateSale;

public class CreateSaleCommandHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IEventSourcingHandler<SaleAggregate> _eventSourcingHandler;
    private readonly IEventPublisher _eventPublisher;
    private readonly CreateSaleCommandHandler _handler;

    public CreateSaleCommandHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _eventSourcingHandler = Substitute.For<IEventSourcingHandler<SaleAggregate>>();
        _eventPublisher = Substitute.For<IEventPublisher>();
        _handler = new CreateSaleCommandHandler(_saleRepository, _mapper, _eventSourcingHandler, _eventPublisher);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateSaleSuccessfully()
    {
        // Arrange
        var command = new CreateSaleCommandBuilder().Build();
        var sale = new SaleBuilder().Build();
        var createdSale = new SaleBuilder().Build();
        // Simulate repository setting the ID
        typeof(Sale).GetProperty("Id")?.SetValue(createdSale, 1);
        var expectedDto = new SaleDto { Id = 1, SaleNumber = createdSale.SaleNumber };

        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.CreateAsync(sale, Arg.Any<CancellationToken>()).Returns(createdSale);
        _mapper.Map<SaleDto>(createdSale).Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(1);
        result.SaleNumber.ShouldBe(createdSale.SaleNumber);
        
        await _saleRepository.Received(1).CreateAsync(sale, Arg.Any<CancellationToken>());
        await _eventSourcingHandler.Received(1).SaveAsync(Arg.Any<SaleAggregate>());
        await _eventPublisher.Received(1).PublishAsync(Arg.Any<SaleCreatedEvent>());
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldMapCommandToSaleCorrectly()
    {
        // Arrange
        var command = new CreateSaleCommandBuilder()
            .WithSaleNumber("SALE-001")
            .WithCustomer("Test Customer")
            .WithBranch("Test Branch")
            .Build();
        
        var sale = new SaleBuilder().Build();
        var createdSale = new SaleBuilder().Build();
        // Simulate repository setting the ID
        typeof(Sale).GetProperty("Id")?.SetValue(createdSale, 1);
        var expectedDto = new SaleDto();

        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.CreateAsync(sale, Arg.Any<CancellationToken>()).Returns(createdSale);
        _mapper.Map<SaleDto>(createdSale).Returns(expectedDto);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mapper.Received(1).Map<Sale>(command);
        _mapper.Received(1).Map<SaleDto>(createdSale);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateAggregateWithCorrectData()
    {
        // Arrange
        var command = new CreateSaleCommandBuilder().Build();
        var sale = new SaleBuilder().Build();
        var createdSale = new SaleBuilder().Build();
        // Simulate repository setting the ID
        typeof(Sale).GetProperty("Id")?.SetValue(createdSale, 123);
        var expectedDto = new SaleDto();

        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.CreateAsync(sale, Arg.Any<CancellationToken>()).Returns(createdSale);
        _mapper.Map<SaleDto>(createdSale).Returns(expectedDto);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _eventSourcingHandler.Received(1).SaveAsync(Arg.Is<SaleAggregate>(agg => 
            agg.SaleId == 123 &&
            agg.SaleNumber == createdSale.SaleNumber &&
            agg.Customer == createdSale.Customer &&
            agg.Branch == createdSale.Branch));
    }
}