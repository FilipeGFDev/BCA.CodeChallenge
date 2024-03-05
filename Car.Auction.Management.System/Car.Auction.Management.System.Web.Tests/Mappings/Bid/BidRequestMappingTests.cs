namespace Car.Auction.Management.System.Web.Tests.Mappings.Bid;

using AutoFixture;
using AutoMapper;
using Car.Auction.Management.System.Application.UseCases.Bid.Create;
using Car.Auction.Management.System.Contracts.Web.Bid;
using Car.Auction.Management.System.Models.Aggregates.Bid;
using Car.Auction.Management.System.Web.Mappings.Bid;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class BidRequestMappingTests
{
    private readonly IFixture _fixture;
    private readonly IMapper _mapper;

    public BidRequestMappingTests()
    {
        _fixture = new Fixture();

        var provider = new ServiceCollection()
            .AddAutoMapper(cfg => cfg.AddProfile(new BidRequestMapping()))
            .BuildServiceProvider();

        _mapper = provider.GetRequiredService<IMapper>();
    }

    [Fact]
    public void OnMap_GivenBidRequest_ShouldReturnExpectedInput()
    {
        // Arrange
        var request = _fixture.Create<BidRequest>();

        var expectedProposal = new BidProposal
        {
            Amount = request.Amount,
            AuctionId = request.AuctionId,
            UserId = request.UserId,
        };

        var expectedInput = new CreateBidInput(expectedProposal);

        // Act
        var result = _mapper.Map<CreateBidInput>(request);

        // Assert
        result.Should().Be(expectedInput);
    }

    [Fact]
    public void OnMap_GivenANullBidRequest_ShouldReturnNull()
    {
        // Arrange & Act
        var result = _mapper.Map<CreateBidInput>(null);

        // Assert
        result.Should().Be(null);
    }
}