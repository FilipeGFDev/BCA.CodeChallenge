namespace Car.Auction.Management.System.Web.Tests.Mappings.Auction;

using AutoFixture;
using AutoMapper;
using Car.Auction.Management.System.Application.UseCases.Auction.Create;
using Car.Auction.Management.System.Contracts.Web.Auction;
using Car.Auction.Management.System.Models.Aggregates.Auction;
using Car.Auction.Management.System.Web.Mappings.Auction;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class AuctionRequestMappingTests
{
    private readonly IFixture _fixture;
    private readonly IMapper _mapper;

    public AuctionRequestMappingTests()
    {
        _fixture = new Fixture();

        var provider = new ServiceCollection()
            .AddAutoMapper(cfg => cfg.AddProfile(new AuctionRequestMapping()))
            .BuildServiceProvider();

        _mapper = provider.GetRequiredService<IMapper>();
    }

    [Fact]
    public void OnMap_GivenAuctionRequest_ShouldReturnExpectedInput()
    {
        // Arrange
        var request = _fixture.Create<AuctionRequest>();

        var expectedProposal = new AuctionProposal
        {
            Description = request.Description,
            VehicleId = request.VehicleId,
        };

        var expectedInput = new CreateAuctionInput(expectedProposal);

        // Act
        var result = _mapper.Map<CreateAuctionInput>(request);

        // Assert
        result.Should().Be(expectedInput);
    }

    [Fact]
    public void OnMap_GivenANullAuctionRequest_ShouldReturnNull()
    {
        // Arrange & Act
        var result = _mapper.Map<CreateAuctionInput>(null);

        // Assert
        result.Should().Be(null);
    }
}