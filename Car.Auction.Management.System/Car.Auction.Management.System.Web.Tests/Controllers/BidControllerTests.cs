namespace Car.Auction.Management.System.Web.Tests.Controllers;

using AutoFixture;
using AutoMapper;
using Car.Auction.Management.System.Application.Core;
using Car.Auction.Management.System.Application.UseCases.Bid.Create;
using Car.Auction.Management.System.Contracts.Web.Bid;
using Car.Auction.Management.System.Web.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class BidControllerTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly BidController _controller;

    public BidControllerTests()
    {
        _fixture = new Fixture();
        _mediatorMock = new();
        _mapperMock = new();
        _controller = new(_mediatorMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task OnCreateAsync_GivenParameters_ShouldReturnCreatedResult()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var request = _fixture.Create<BidRequest>();
        var input = _fixture.Create<CreateBidInput>();

        var useCaseResult = new UseCaseResult<BidCreatedUseCaseEvent>(new(id));

        _mapperMock
            .Setup(x => x.Map<CreateBidInput>(request))
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
            .Be($"/bid/{id}");
    }
}