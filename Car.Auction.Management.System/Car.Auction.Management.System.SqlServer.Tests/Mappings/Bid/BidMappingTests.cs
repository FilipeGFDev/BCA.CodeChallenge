namespace Car.Auction.Management.System.SqlServer.Tests.Mappings.Bid;

using AutoFixture;
using AutoMapper;
using Car.Auction.Management.System.Contracts.Web.Bid.Get;
using Car.Auction.Management.System.Models.Aggregates.Bid;
using Car.Auction.Management.System.SqlServer.Mappings.Bid;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class BidMappingTests
{
    private readonly IFixture _fixture;
    private readonly IMapper _mapper;

    public BidMappingTests()
    {
        _fixture = new Fixture();

        var provider = new ServiceCollection()
            .AddAutoMapper(cfg => cfg.AddProfile(new BidMapping()))
            .BuildServiceProvider();

        _mapper = provider.GetRequiredService<IMapper>();
    }

    [Fact]
    public void OnMap_GivenBid_ShouldReturnExpectedResponse()
    {
        // Arrange
        var proposal = _fixture.Create<BidProposal>();
        var entity = new Bid(proposal);
        var expectedResponse = new GetBidResponse(
            entity.Id,
            entity.UserId,
            entity.Amount,
            entity.CreatedAt);

        // Act
        var result = _mapper.Map<GetBidResponse>(entity);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public void OnMap_GivenANullBid_ShouldReturnNull()
    {
        // Arrange & Act
        var result = _mapper.Map<GetBidResponse>(null);

        // Assert
        result.Should().Be(null);
    }
}