namespace Car.Auction.Management.System.SqlServer.Tests.Mappings.Vehicle;

using AutoFixture;
using AutoMapper;
using Car.Auction.Management.System.Contracts.Web.Vehicle;
using Car.Auction.Management.System.Contracts.Web.Vehicle.Get;
using Car.Auction.Management.System.Models.Aggregates.Vehicle;
using Car.Auction.Management.System.SqlServer.Mappings.Vehicle;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class VehicleMappingTests
{
    private readonly IFixture _fixture;
    private readonly IMapper _mapper;

    public VehicleMappingTests()
    {
        _fixture = new Fixture();

        var provider = new ServiceCollection()
            .AddScoped<VehicleConverter>()
            .AddAutoMapper(cfg => cfg.AddProfile(new VehicleMapping()))
            .BuildServiceProvider();

        _mapper = provider.GetRequiredService<IMapper>();
    }

    [Fact]
    public void OnMap_GivenAHatchback_ShouldReturnExpectedResponse()
    {
        // Arrange
        var proposal = _fixture.Create<VehicleProposal>();
        var entity = new Hatchback(proposal);
        var expectedResponse = new GetVehicleResponse(
            entity.Id,
            VehicleType.Hatchback,
            entity.Manufacturer,
            entity.Model,
            entity.Year,
            entity.StartingBid,
            entity.IsAvailable,
            entity.DoorsNumber,
            null,
            null);

        // Act
        var result = _mapper.Map<GetVehicleResponse>(entity);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public void OnMap_GivenASedan_ShouldReturnExpectedResponse()
    {
        // Arrange
        var proposal = _fixture.Create<VehicleProposal>();
        var entity = new Sedan(proposal);
        var expectedResponse = new GetVehicleResponse(
            entity.Id,
            VehicleType.Sedan,
            entity.Manufacturer,
            entity.Model,
            entity.Year,
            entity.StartingBid,
            entity.IsAvailable,
            entity.DoorsNumber,
            null,
            null);

        // Act
        var result = _mapper.Map<GetVehicleResponse>(entity);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public void OnMap_GivenASuv_ShouldReturnExpectedResponse()
    {
        // Arrange
        var proposal = _fixture.Create<VehicleProposal>();
        var entity = new Suv(proposal);
        var expectedResponse = new GetVehicleResponse(
            entity.Id,
            VehicleType.Suv,
            entity.Manufacturer,
            entity.Model,
            entity.Year,
            entity.StartingBid,
            entity.IsAvailable,
            null,
            null,
            entity.SeatsNumber);

        // Act
        var result = _mapper.Map<GetVehicleResponse>(entity);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public void OnMap_GivenATruck_ShouldReturnExpectedResponse()
    {
        // Arrange
        var proposal = _fixture.Create<VehicleProposal>();
        var entity = new Truck(proposal);
        var expectedResponse = new GetVehicleResponse(
            entity.Id,
            VehicleType.Truck,
            entity.Manufacturer,
            entity.Model,
            entity.Year,
            entity.StartingBid,
            entity.IsAvailable,
            null,
            entity.LoadCapacity,
            null);

        // Act
        var result = _mapper.Map<GetVehicleResponse>(entity);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public void OnMap_GivenANullVehicle_ShouldReturnNull()
    {
        // Arrange & Act
        var result = _mapper.Map<GetVehicleResponse>(null);

        // Assert
        result.Should().Be(null);
    }
}