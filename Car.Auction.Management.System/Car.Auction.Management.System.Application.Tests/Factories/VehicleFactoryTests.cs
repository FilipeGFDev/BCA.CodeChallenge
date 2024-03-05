namespace Car.Auction.Management.System.Application.Tests.Factories;

using AutoFixture;
using Car.Auction.Management.System.Application.Factories;
using Car.Auction.Management.System.Contracts.Web.Vehicle;
using Car.Auction.Management.System.Models.Aggregates.Vehicle;
using FluentAssertions;
using Xunit;

public class VehicleFactoryTests
{
    private readonly IFixture _fixture;

    public VehicleFactoryTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void GivenVehicleProposal_WithHatchbackType_ShouldReturnHatchbackInstance()
    {
        // Arrange
        var proposal = _fixture.Build<VehicleProposal>()
            .With(x => x.VehicleType, VehicleType.Hatchback)
            .Create();

        var expected = new Hatchback(proposal);

        // Act
        var result = VehicleFactory.CreateVehicle(proposal);

        // Assert
        result.Should().BeOfType<Hatchback>();
        result
            .Should()
            .BeEquivalentTo(
                expected,
                opt =>
                    opt.Excluding(x => x.Id)
                        .Excluding(x => x.CreatedAt));
    }

    [Fact]
    public void GivenVehicleProposal_WithSedanType_ShouldReturnSedanInstance()
    {
        // Arrange
        var proposal = _fixture.Build<VehicleProposal>()
            .With(x => x.VehicleType, VehicleType.Sedan)
            .Create();

        var expected = new Sedan(proposal);

        // Act
        var result = VehicleFactory.CreateVehicle(proposal);

        // Assert
        result.Should().BeOfType<Sedan>();
        result
            .Should()
            .BeEquivalentTo(
                expected,
                opt =>
                    opt.Excluding(x => x.Id)
                        .Excluding(x => x.CreatedAt));
    }

    [Fact]
    public void GivenVehicleProposal_WithSuvType_ShouldReturnSuvInstance()
    {
        // Arrange
        var proposal = _fixture.Build<VehicleProposal>()
            .With(x => x.VehicleType, VehicleType.Suv)
            .Create();

        var expected = new Suv(proposal);

        // Act
        var result = VehicleFactory.CreateVehicle(proposal);

        // Assert
        result.Should().BeOfType<Suv>();
        result
            .Should()
            .BeEquivalentTo(
                expected,
                opt =>
                    opt.Excluding(x => x.Id)
                        .Excluding(x => x.CreatedAt));
    }

    [Fact]
    public void GivenVehicleProposal_WithTruckType_ShouldReturnTruckInstance()
    {
        // Arrange
        var proposal = _fixture.Build<VehicleProposal>()
            .With(x => x.VehicleType, VehicleType.Truck)
            .Create();

        var expected = new Truck(proposal);

        // Act
        var result = VehicleFactory.CreateVehicle(proposal);

        // Assert
        result.Should().BeOfType<Truck>();
        result
            .Should()
            .BeEquivalentTo(
                expected,
                opt =>
                    opt.Excluding(x => x.Id)
                        .Excluding(x => x.CreatedAt));
    }
}