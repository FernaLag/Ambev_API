using Ambev.Ominia.Application.Features.Sales;
using Ambev.Ominia.Application.Features.Sales.Commands.UpdateSale;
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

namespace Ambev.Ominia.Application.Tests.Features.Sales.Commands.UpdateSale;

public class UpdateSaleCommandHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IEventSourcingHandler<SaleAggregate> _eventSourcingHandler;
    private readonly IEventPublisher _eventPublisher;
    private readonly UpdateSaleCommandHandler _handler;

    public UpdateSaleCommandHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _eventSourcingHandler = Substitute.For<IEventSourcingHandler<SaleAggregate>>();
        _eventPublisher = Substitute.For<IEventPublisher>();
        _handler = new UpdateSaleCommandHandler(_saleRepository, _mapper, _eventSourcingHandler, _eventPublisher);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldUpdateSaleSuccessfully()
    {
        // Arrange
        var command = new UpdateSaleCommandBuilder().WithId(1).Build();
        var existingSale = new SaleBuilder().Build();
        var updatedSale = new SaleBuilder().Build();
        var saleAggregate = new SaleAggregate();
        var expectedDto = new SaleDto { Id = 1, SaleNumber = updatedSale.SaleNumber };

        // Simulate repository setting the ID
        typeof(Sale).GetProperty("Id")?.SetValue(existingSale, 1);
        typeof(Sale).GetProperty("Id")?.SetValue(updatedSale, 1);

        _saleRepository.GetByIdAsync(1, Arg.Any<CancellationToken>()).Returns(existingSale);
        _saleRepository.UpdateAsync(existingSale, Arg.Any<CancellationToken>()).Returns(updatedSale);
        _eventSourcingHandler.GetByIdAsync(Arg.Any<Guid>()).Returns(saleAggregate);
        _mapper.Map<SaleDto>(updatedSale).Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(1);
        result.SaleNumber.ShouldBe(updatedSale.SaleNumber);
        
        await _saleRepository.Received(1).GetByIdAsync(1, Arg.Any<CancellationToken>());
        await _saleRepository.Received(1).UpdateAsync(existingSale, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map(command, existingSale);
        await _eventSourcingHandler.Received(1).SaveAsync(Arg.Any<SaleAggregate>());
        await _eventPublisher.Received(1).PublishAsync(Arg.Any<SaleModifiedEvent>());
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldMapCommandToExistingSale()
    {
        // Arrange
        var command = new UpdateSaleCommandBuilder()
            .WithId(1)
            .WithSaleNumber("UPDATED-SALE-001")
            .WithCustomer("Updated Customer")
            .Build();
        
        var existingSale = new SaleBuilder().Build();
        var updatedSale = new SaleBuilder().Build();
        var saleAggregate = new SaleAggregate();
        var expectedDto = new SaleDto();

        typeof(Sale).GetProperty("Id")?.SetValue(existingSale, 1);
        typeof(Sale).GetProperty("Id")?.SetValue(updatedSale, 1);

        _saleRepository.GetByIdAsync(1, Arg.Any<CancellationToken>()).Returns(existingSale);
        _saleRepository.UpdateAsync(existingSale, Arg.Any<CancellationToken>()).Returns(updatedSale);
        _eventSourcingHandler.GetByIdAsync(Arg.Any<Guid>()).Returns(saleAggregate);
        _mapper.Map<SaleDto>(updatedSale).Returns(expectedDto);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mapper.Received(1).Map(command, existingSale);
        _mapper.Received(1).Map<SaleDto>(updatedSale);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldUpdateAggregateAndPublishEvent()
    {
        // Arrange
        var command = new UpdateSaleCommandBuilder().WithId(1).Build();
        var existingSale = new SaleBuilder().Build();
        var updatedSale = new SaleBuilder().Build();
        var saleAggregate = new SaleAggregate();
        var expectedDto = new SaleDto();

        typeof(Sale).GetProperty("Id")?.SetValue(existingSale, 1);
        typeof(Sale).GetProperty("Id")?.SetValue(updatedSale, 1);

        _saleRepository.GetByIdAsync(1, Arg.Any<CancellationToken>()).Returns(existingSale);
        _saleRepository.UpdateAsync(existingSale, Arg.Any<CancellationToken>()).Returns(updatedSale);
        _eventSourcingHandler.GetByIdAsync(Arg.Any<Guid>()).Returns(saleAggregate);
        _mapper.Map<SaleDto>(updatedSale).Returns(expectedDto);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _eventSourcingHandler.Received(1).GetByIdAsync(Arg.Any<Guid>());
        await _eventSourcingHandler.Received(1).SaveAsync(saleAggregate);
        await _eventPublisher.Received(1).PublishAsync(Arg.Any<SaleModifiedEvent>());
    }
}