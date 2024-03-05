namespace Car.Auction.Management.System.SqlServer.Tests.Mappings.Auction;

using AutoFixture;
using AutoMapper;
using Car.Auction.Management.System.Contracts.Web.Auction.Get;
using Car.Auction.Management.System.Contracts.Web.Bid.Get;
using Car.Auction.Management.System.Contracts.Web.Vehicle.Get;
using Car.Auction.Management.System.Models.Aggregates.Auction;
using Car.Auction.Management.System.SqlServer.Mappings.Auction;
using Car.Auction.Management.System.SqlServer.Mappings.Bid;
using Car.Auction.Management.System.SqlServer.Mappings.Vehicle;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class AuctionMappingTests
{
    private readonly IFixture _fixture;
    private readonly IMapper _mapper;

    public AuctionMappingTests()
    {
        _fixture = new Fixture();

        var provider = new ServiceCollection()
            .AddAutoMapper(cfg =>
            {
                cfg.AddProfile(new AuctionMapping());
                cfg.AddProfile(new VehicleMapping());
                cfg.AddProfile(new BidMapping());
            })
            .BuildServiceProvider();

        _mapper = provider.GetRequiredService<IMapper>();
    }

    [Fact]
    public void OnMap_GivenAuction_ShouldReturnExpectedResponse()
    {
        // Arrange
        var proposal = _fixture.Create<AuctionProposal>();
        var entity = new Auction(proposal);
        var expectedResponse = new GetAuctionResponse(
            entity.Id,
            entity.Description,
            entity.StartedAt,
            entity.ClosedAt,
            entity.IsActive,
            _mapper.Map<GetVehicleResponse>(entity.Vehicle),
            _mapper.Map<List<GetBidResponse>>(entity.Bids));

        // Act
        var result = _mapper.Map<GetAuctionResponse>(entity);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public void OnMap_GivenANullAuction_ShouldReturnNull()
    {
        // Arrange & Act
        var result = _mapper.Map<GetAuctionResponse>(null);

        // Assert
        result.Should().Be(null);
    }
}