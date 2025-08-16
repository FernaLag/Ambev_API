using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Xunit;
using Ambev.Ominia.Api.Controllers;
using Ambev.Ominia.Application.Features.Sales.Commands.CancelItem;
using Ambev.Ominia.Application.Features.Sales.Commands.CancelSale;
using Ambev.Ominia.Application.Features.Sales.Commands.CreateSale;
using Ambev.Ominia.Application.Features.Sales.Commands.DeleteSale;
using Ambev.Ominia.Application.Features.Sales.Commands.UpdateSale;
using Ambev.Ominia.Application.Features.Sales.Queries.GetSale;
using Ambev.Ominia.Application.Features.Sales.Queries.ListSales;
using Ambev.Ominia.Application.Features.Sales;
using Ambev.Ominia.Domain.Entities.Sales;
using Ambev.Ominia.Domain.Responses;
using Bogus;

namespace Ambev.Ominia.Api.Tests.Controllers;

/// <summary>
/// Tests for <see cref="SalesController"/>
/// </summary>
public class SalesControllerTests
{
    private readonly IMediator _mediator;
    private readonly SalesController _controller;
    private readonly Faker _faker;

    public SalesControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
        _controller = new SalesController(_mediator);
        _faker = new Faker();
        
        // Configure ControllerContext to avoid NullReferenceException
        _controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
        {
            ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor
            {
                ControllerName = "Sales"
            }
        };
    }

    #region ListSales Tests

    [Fact]
    public async Task ListSales_ValidQuery_ShouldReturnOkWithPaginatedResponse()
    {
        // Arrange
        var query = new ListSalesQuery { Page = 1, PageSize = 10 };
        var salesList = new List<SaleSummaryDto>
        {
            new SaleSummaryDto(1, "SALE-001", DateTime.Now, "Customer 1", "Branch 1"),
            new SaleSummaryDto(2, "SALE-002", DateTime.Now, "Customer 2", "Branch 2")
        };

        _mediator.Send(query, Arg.Any<CancellationToken>()).Returns(salesList);

        // Act
        var result = await _controller.ListSales(query);

        // Assert
        result.ShouldBeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.ShouldBeOfType<PaginatedResponse>();

        await _mediator.Received(1).Send(query, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ListSales_EmptyResult_ShouldReturnOkWithEmptyPaginatedResponse()
    {
        // Arrange
        var query = new ListSalesQuery { Page = 1, PageSize = 10 };
        var emptySalesList = new List<SaleSummaryDto>();

        _mediator.Send(query, Arg.Any<CancellationToken>()).Returns(emptySalesList);

        // Act
        var result = await _controller.ListSales(query);

        // Assert
        result.ShouldBeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.ShouldBeOfType<PaginatedResponse>();

        await _mediator.Received(1).Send(query, Arg.Any<CancellationToken>());
    }

    #endregion

    #region GetSale Tests

    [Fact]
    public async Task GetSale_ValidId_ShouldReturnOkWithSaleDto()
    {
        // Arrange
        var saleId = 1;
        var expectedSale = new SaleDto
        {
            Id = saleId,
            SaleNumber = "SALE-001",
            Date = DateTime.Now,
            CustomerName = "Test Customer",
            BranchName = "Test Branch",
            Items = new List<SaleItemDto>()
        };

        _mediator.Send(Arg.Any<GetSaleQuery>(), Arg.Any<CancellationToken>()).Returns(expectedSale);

        // Act
        var result = await _controller.GetSale(saleId);

        // Assert
        result.ShouldBeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.ShouldBeOfType<Response>();

        await _mediator.Received(1).Send(Arg.Is<GetSaleQuery>(q => q.Id == saleId), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetSale_ValidId_ShouldCreateCorrectQuery()
    {
        // Arrange
        var saleId = 123;
        var expectedSale = new SaleDto { Id = saleId };

        _mediator.Send(Arg.Any<GetSaleQuery>(), Arg.Any<CancellationToken>()).Returns(expectedSale);

        // Act
        await _controller.GetSale(saleId);

        // Assert
        await _mediator.Received(1).Send(
            Arg.Is<GetSaleQuery>(q => q.Id == saleId),
            Arg.Any<CancellationToken>()
        );
    }

    #endregion

    #region CreateSale Tests

    [Fact]
    public async Task CreateSale_ValidCommand_ShouldReturnCreatedWithSaleDto()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            SaleNumber = "SALE-NEW",
            Date = DateTime.Now,
            Customer = "New Customer",
            Branch = "New Branch",
            Items = [new SaleItem(0, 1, 2, 25.00m, 0m)]
            };

        var expectedSale = new SaleDto
        {
            Id = 1,
            SaleNumber = command.SaleNumber,
            CustomerName = command.Customer,
            BranchName = command.Branch
        };

        _mediator.Send(command, Arg.Any<CancellationToken>()).Returns(expectedSale);

        // Act
        var result = await _controller.CreateSale(command);

        // Assert
        result.ShouldBeOfType<CreatedResult>();
        var createdResult = result as CreatedResult;
        createdResult!.Value.ShouldBeOfType<Response>();

        await _mediator.Received(1).Send(command, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateSale_ValidCommand_ShouldPassCommandToMediator()
    {
        // Arrange
        var command = new CreateSaleCommand { SaleNumber = "SALE-TEST", Date = DateTime.Now, Customer = "Customer", Branch = "Branch", Items =
                    []
            };
        var expectedSale = new SaleDto();

        _mediator.Send(command, Arg.Any<CancellationToken>()).Returns(expectedSale);

        // Act
        await _controller.CreateSale(command);

        // Assert
        await _mediator.Received(1).Send(command, Arg.Any<CancellationToken>());
    }

    #endregion

    #region UpdateSale Tests

    [Fact]
    public async Task UpdateSale_ValidCommand_ShouldReturnOkWithSaleDto()
    {
        // Arrange
        var saleId = 1;
        var command = new UpdateSaleCommand
        {
            Id = 0, // Will be overridden by route parameter
            Customer = "Updated Customer",
            Branch = "Updated Branch",
            Items = [new SaleItem(0, 1, 3, 30.00m, 5.00m)]
            };

        var expectedSale = new SaleDto
        {
            Id = saleId
        };

        _mediator.Send(Arg.Any<UpdateSaleCommand>(), Arg.Any<CancellationToken>()).Returns(expectedSale);

        // Act
        var result = await _controller.UpdateSale(saleId, command);

        // Assert
        result.ShouldBeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.ShouldBeOfType<Response>();

        // Verify that the command ID was set from the route parameter
        await _mediator.Received(1).Send(
            Arg.Is<UpdateSaleCommand>(c => c.Id == saleId),
            Arg.Any<CancellationToken>()
        );
    }

    [Fact]
    public async Task UpdateSale_ShouldSetIdFromRouteParameter()
    {
        // Arrange
        var saleId = 456;
        var command = new UpdateSaleCommand { Id = 0, Customer = "Customer", Branch = "Branch", Items = [] };
        var expectedSale = new SaleDto();

        _mediator.Send(Arg.Any<UpdateSaleCommand>(), Arg.Any<CancellationToken>()).Returns(expectedSale);

        // Act
        await _controller.UpdateSale(saleId, command);

        // Assert
        command.Id.ShouldBe(saleId);
        await _mediator.Received(1).Send(command, Arg.Any<CancellationToken>());
    }

    #endregion

    #region DeleteSale Tests

    [Fact]
    public async Task DeleteSale_ValidId_ShouldReturnOkWithSuccessMessage()
    {
        // Arrange
        var saleId = 1;

        // Act
        var result = await _controller.DeleteSale(saleId);

        // Assert
        result.ShouldBeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.ShouldBeOfType<Response>();
        var response = okResult.Value as Response;
        response!.Data.ShouldBe("Sale deleted successfully");

        await _mediator.Received(1).Send(
            Arg.Is<DeleteSaleCommand>(c => c.Id == saleId),
            Arg.Any<CancellationToken>()
        );
    }

    [Fact]
    public async Task DeleteSale_ShouldCreateCorrectCommand()
    {
        // Arrange
        var saleId = 789;

        // Act
        await _controller.DeleteSale(saleId);

        // Assert
        await _mediator.Received(1).Send(
            Arg.Is<DeleteSaleCommand>(c => c.Id == saleId),
            Arg.Any<CancellationToken>()
        );
    }

    #endregion

    #region CancelSale Tests

    [Fact]
    public async Task CancelSale_ValidId_ShouldReturnOkWithSaleDto()
    {
        // Arrange
        var saleId = 1;
        var expectedSale = new SaleDto
        {
            Id = saleId,
            SaleNumber = "SALE-001"
        };

        _mediator.Send(Arg.Any<CancelSaleCommand>(), Arg.Any<CancellationToken>()).Returns(expectedSale);

        // Act
        var result = await _controller.CancelSale(saleId);

        // Assert
        result.ShouldBeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.ShouldBeOfType<Response>();

        await _mediator.Received(1).Send(
            Arg.Is<CancelSaleCommand>(c => c.Id == saleId),
            Arg.Any<CancellationToken>()
        );
    }

    [Fact]
    public async Task CancelSale_ShouldCreateCorrectCommand()
    {
        // Arrange
        var saleId = 321;
        var expectedSale = new SaleDto();

        _mediator.Send(Arg.Any<CancelSaleCommand>(), Arg.Any<CancellationToken>()).Returns(expectedSale);

        // Act
        await _controller.CancelSale(saleId);

        // Assert
        await _mediator.Received(1).Send(
            Arg.Is<CancelSaleCommand>(c => c.Id == saleId),
            Arg.Any<CancellationToken>()
        );
    }

    #endregion

    #region CancelItem Tests

    [Fact]
    public async Task CancelItem_ValidIds_ShouldReturnOkWithSaleDto()
    {
        // Arrange
        var saleId = 1;
        var itemId = 2;
        var expectedSale = new SaleDto
        {
            Id = saleId,
            SaleNumber = "SALE-001"
        };

        _mediator.Send(Arg.Any<CancelItemCommand>(), Arg.Any<CancellationToken>()).Returns(expectedSale);

        // Act
        var result = await _controller.CancelItem(saleId, itemId);

        // Assert
        result.ShouldBeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.ShouldBeOfType<Response>();

        await _mediator.Received(1).Send(
            Arg.Is<CancelItemCommand>(c => c.SaleId == saleId && c.ItemId == itemId),
            Arg.Any<CancellationToken>()
        );
    }

    [Fact]
    public async Task CancelItem_ShouldCreateCorrectCommand()
    {
        // Arrange
        var saleId = 123;
        var itemId = 456;
        var expectedSale = new SaleDto();

        _mediator.Send(Arg.Any<CancelItemCommand>(), Arg.Any<CancellationToken>()).Returns(expectedSale);

        // Act
        await _controller.CancelItem(saleId, itemId);

        // Assert
        await _mediator.Received(1).Send(
            Arg.Is<CancelItemCommand>(c => c.SaleId == saleId && c.ItemId == itemId),
            Arg.Any<CancellationToken>()
        );
    }

    #endregion



    #region Integration Tests

    [Fact]
    public async Task Controller_AllMethods_ShouldUseMediatorCorrectly()
    {
        // Arrange
        var listQuery = new ListSalesQuery();
        var createCommand = new CreateSaleCommand { SaleNumber = "SALE-001", Date = DateTime.Now, Customer = "Customer", Branch = "Branch", Items =
                    []
            };
        var updateCommand = new UpdateSaleCommand { Id = 0, Customer = "Customer", Branch = "Branch", Items = [] };

        var salesList = new List<SaleSummaryDto>();
        var saleDto = new SaleDto();

        _mediator.Send(listQuery, Arg.Any<CancellationToken>()).Returns(salesList);
        _mediator.Send(Arg.Any<GetSaleQuery>(), Arg.Any<CancellationToken>()).Returns(saleDto);
        _mediator.Send(createCommand, Arg.Any<CancellationToken>()).Returns(saleDto);
        _mediator.Send(Arg.Any<UpdateSaleCommand>(), Arg.Any<CancellationToken>()).Returns(saleDto);
        _mediator.Send(Arg.Any<CancelSaleCommand>(), Arg.Any<CancellationToken>()).Returns(saleDto);
        _mediator.Send(Arg.Any<CancelItemCommand>(), Arg.Any<CancellationToken>()).Returns(saleDto);

        // Act
        await _controller.ListSales(listQuery);
        await _controller.GetSale(1);
        await _controller.CreateSale(createCommand);
        await _controller.UpdateSale(1, updateCommand);
        await _controller.DeleteSale(1);
        await _controller.CancelSale(1);
        await _controller.CancelItem(1, 1);

        // Assert
        await _mediator.Received(1).Send(listQuery, Arg.Any<CancellationToken>());
        await _mediator.Received(1).Send(Arg.Any<GetSaleQuery>(), Arg.Any<CancellationToken>());
        await _mediator.Received(1).Send(createCommand, Arg.Any<CancellationToken>());
        await _mediator.Received(1).Send(Arg.Any<UpdateSaleCommand>(), Arg.Any<CancellationToken>());
        await _mediator.Received(1).Send(Arg.Any<DeleteSaleCommand>(), Arg.Any<CancellationToken>());
        await _mediator.Received(1).Send(Arg.Any<CancelSaleCommand>(), Arg.Any<CancellationToken>());
        await _mediator.Received(1).Send(Arg.Any<CancelItemCommand>(), Arg.Any<CancellationToken>());
    }

    #endregion
}