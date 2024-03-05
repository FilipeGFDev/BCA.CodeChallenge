namespace Car.Auction.Management.System.Web.Tests.Controllers;

using AutoFixture;
using AutoMapper;
using Car.Auction.Management.System.Application.Core;
using Car.Auction.Management.System.Application.UseCases.Vehicle.Create;
using Car.Auction.Management.System.Contracts.Web.Paged;
using Car.Auction.Management.System.Contracts.Web.Vehicle;
using Car.Auction.Management.System.Contracts.Web.Vehicle.Get;
using Car.Auction.Management.System.Web.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class VehicleControllerTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly VehicleController _controller;

    public VehicleControllerTests()
    {
        _fixture = new Fixture();
        _mediatorMock = new();
        _mapperMock = new();
        _controller = new(_mediatorMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_GivenAValidId_ShouldReturnOkResultWithExpectedInformation()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var expectedResult = _fixture.Create<GetVehicleResponse>();

        _mediatorMock
            .Setup(x => x.Send(
                It.Is<GetVehicleByIdRequest>(r => r.VehicleId == id),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetByIdAsync(id);

        // Assert
        result
            .Should()
            .BeOfType<OkObjectResult>()
            .Which
            .Value
            .Should()
            .BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetAllAsync_GivenAValidId_ShouldReturnOkResultWithExpectedInformation()
    {
        // Arrange
        var requestFilter = _fixture.Create<GetVehiclesRequest.RequestFilter>();
        var expectedResult = _fixture.Create<PagedResult<GetVehicleResponse>>();

        _mediatorMock
            .Setup(x => x.Send(
                It.Is<GetVehiclesRequest>(r => r.Filter == requestFilter),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetAllAsync(requestFilter);

        // Assert
        result
            .Should()
            .BeOfType<OkObjectResult>()
            .Which
            .Value
            .Should()
            .BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task OnCreateAsync_GivenParameters_ShouldReturnCreatedResult()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var request = _fixture.Create<VehicleRequest>();
        var input = _fixture.Create<CreateVehicleInput>();

        var useCaseResult = new UseCaseResult<VehicleCreatedUseCaseEvent>(new(id));

        _mapperMock
            .Setup(x => x.Map<CreateVehicleInput>(request))
            .Returns(input);

        _mediatorMock
            .Setup(o => o.Send(input, It.IsAny<CancellationToken>()))
            .ReturnsAsync(useCaseResult);

        // Act
        var result = await _controller.CreateAsync(request);

        // Assert
        result
            .Should()
            .BeOfType<CreatedResult>()
            .Which
            .Location
            .Should()
            .Be($"/vehicle/{id}");
    }
}