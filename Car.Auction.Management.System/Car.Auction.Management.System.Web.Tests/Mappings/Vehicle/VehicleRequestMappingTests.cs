namespace Car.Auction.Management.System.Web.Tests.Mappings.Vehicle;

using AutoFixture;
using AutoMapper;
using Car.Auction.Management.System.Application.UseCases.Vehicle.Create;
using Car.Auction.Management.System.Contracts.Web.Vehicle;
using Car.Auction.Management.System.Models.Aggregates.Vehicle;
using Car.Auction.Management.System.Web.Mappings.Vehicle;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class VehicleRequestMappingTests
{
    private readonly IFixture _fixture;
    private readonly IMapper _mapper;

    public VehicleRequestMappingTests()
    {
        _fixture = new Fixture();

        var provider = new ServiceCollection()
            .AddAutoMapper(cfg => cfg.AddProfile(new VehicleRequestMapping()))
            .BuildServiceProvider();

        _mapper = provider.GetRequiredService<IMapper>();
    }

    [Fact]
    public void OnMap_GivenVehicleRequest_ShouldReturnExpectedInput()
    {
        // Arrange
        var request = _fixture.Create<VehicleRequest>();

        var expectedProposal = new VehicleProposal
        {
            VehicleType = request.VehicleType.GetValueOrDefault(),
            Manufacturer = request.Manufacturer,
            Model = request.Model,
            Year = request.Year,
            StartingBid = request.StartingBid,
            DoorsNumber = request.DoorsNumber.GetValueOrDefault(),
            LoadCapacity = request.LoadCapacity.GetValueOrDefault(),
            SeatsNumber = request.SeatsNumber.GetValueOrDefault(),
            
        };

        var expectedInput = new CreateVehicleInput(expectedProposal);

        // Act
        var result = _mapper.Map<CreateVehicleInput>(request);

        // Assert
        result.Should().Be(expectedInput);
    }

    [Fact]
    public void OnMap_GivenANullVehicleRequest_ShouldReturnNull()
    {
        // Arrange & Act
        var result = _mapper.Map<CreateVehicleInput>(null);

        // Assert
        result.Should().Be(null);
    }
}