namespace Car.Auction.Management.System.Application.Tests.Helpers;

using AutoFixture;
using Car.Auction.Management.System.Application.Helpers;
using FluentAssertions;
using Xunit;

public class AuctionHelperTests
{
    private readonly IFixture _fixture;

    public AuctionHelperTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void GivenAuctionOperationPathValid_ShouldReturnTrue()
    {
        // Arrange
        const string path = "/isActive";

        // Act
        var result = AuctionHelper.IsIsActiveFieldOperation(path);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void GivenAuctionOperationPathInvalid_ShouldReturnTrue()
    {
        // Arrange
        var path = _fixture.Create<string>();

        // Act
        var result = AuctionHelper.IsIsActiveFieldOperation(path);

        // Assert
        result.Should().BeFalse();
    }
}