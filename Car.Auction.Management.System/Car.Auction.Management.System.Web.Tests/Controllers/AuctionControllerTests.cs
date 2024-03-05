namespace Car.Auction.Management.System.Web.Tests.Controllers;

using AutoFixture;
using AutoMapper;
using Car.Auction.Management.System.Application.Core;
using Car.Auction.Management.System.Application.UseCases.Auction.Create;
using Car.Auction.Management.System.Application.UseCases.Auction.Update;
using Car.Auction.Management.System.Contracts.Web.Auction;
using Car.Auction.Management.System.Contracts.Web.Auction.Get;
using Car.Auction.Management.System.Models.Aggregates.Auction;
using Car.Auction.Management.System.Web.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class AuctionControllerTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AuctionController _controller;
    
    public AuctionControllerTests()
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
        var expectedResult = _fixture.Create<GetAuctionResponse>();

        _mediatorMock
            .Setup(x => x.Send(
                It.Is<GetAuctionByIdRequest>(r => r.AuctionId == id),
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
    public async Task OnCreateAsync_GivenParameters_ShouldReturnCreatedResult()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var request = _fixture.Create<AuctionRequest>();
        var input = _fixture.Create<CreateAuctionInput>();

        var useCaseResult = new UseCaseResult<AuctionCreatedUseCaseEvent>(new(id));

        _mapperMock
            .Setup(x => x.Map<CreateAuctionInput>(request))
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
            .Be($"/auction/{id}");
    }

    [Fact]
    public async Task OnUpdateAsync_GivenParameters_ShouldReturnNoContentResult()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var request = new JsonPatchDocument<Auction>();
        var input = new UpdateAuctionInput(id, request);

        var useCaseResult = new UseCaseResult<AuctionUpdatedUseCaseEvent>(new());

        _mediatorMock
            .Setup(o => o.Send(input, It.IsAny<CancellationToken>()))
            .ReturnsAsync(useCaseResult);

        // Act
        var result = await _controller.UpdateAsync(id, request);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }
}