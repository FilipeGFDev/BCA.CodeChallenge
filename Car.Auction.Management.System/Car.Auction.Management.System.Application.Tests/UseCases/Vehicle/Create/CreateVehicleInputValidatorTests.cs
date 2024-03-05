namespace Car.Auction.Management.System.Application.Tests.UseCases.Vehicle.Create;

using AutoFixture;
using Car.Auction.Management.System.Application.UseCases.Vehicle.Create;
using Car.Auction.Management.System.Contracts.Web.Vehicle;
using Car.Auction.Management.System.Models.Aggregates.Vehicle;
using Car.Auction.Management.System.Models.Core;
using Car.Auction.Management.System.Models.ErrorCodes;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

public class CreateVehicleInputValidatorTests
{
    private readonly IFixture _fixture;
    private readonly Random _random;
    private readonly CreateVehicleInputValidator _validator;

    public CreateVehicleInputValidatorTests()
    {
        _fixture = new Fixture();
        _random = new();
        _validator = new();
    }

    [Theory]
    [InlineData(VehicleType.Hatchback)]
    [InlineData(VehicleType.Sedan)]
    [InlineData(VehicleType.Suv)]
    [InlineData(VehicleType.Truck)]
    public async void OnValidate_GivenACreateVehicleInput_ShouldNotHaveValidationErrors(VehicleType vehicleType)
    {
        var doorsNumber = new ChangeRequest<int>(vehicleType is VehicleType.Hatchback or VehicleType.Sedan
            ? _random.Next(3, 10)
            : default);
        var seatsNumber = new ChangeRequest<int>(vehicleType is VehicleType.Suv ? _random.Next(1, 10) : default);
        var loadCapacity = new ChangeRequest<int>(vehicleType is VehicleType.Truck ? _random.Next(1, 10) : default);

        var proposal = _fixture.Build<VehicleProposal>()
            .With(x => x.VehicleType, new ChangeRequest<VehicleType>(vehicleType))
            .With(x => x.Year, new ChangeRequest<int>(_random.Next(1885, DateTime.UtcNow.Year)))
            .With(x => x.DoorsNumber, doorsNumber)
            .With(x => x.SeatsNumber, seatsNumber)
            .With(x => x.LoadCapacity, loadCapacity)
            .Create();
        var validatorInput = new CreateVehicleInput(proposal);

        // Act
        var result = await _validator.TestValidateAsync(validatorInput);

        // Assert
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public async void OnValidate_GivenACreateVehicleInput_WhenVehicleTypeIsNull_ShouldHaveValidationErrors()
    {
        var proposal = _fixture.Build<VehicleProposal>()
            .With(x => x.VehicleType, new ChangeRequest<VehicleType>(VehicleType.None))
            .Create();
        var validatorInput = new CreateVehicleInput(proposal);

        // Act
        var result = await _validator.TestValidateAsync(validatorInput);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(x => x.ErrorCode == ErrorCodes.Vehicle.InvalidVehicleType.Code);
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorCodes.Vehicle.InvalidVehicleType.ErrorMessage);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async void OnValidate_GivenACreateVehicleInput_WhenManufacturerIsNull_ShouldHaveValidationErrors(
        string? manufacturer)
    {
        var proposal = _fixture.Build<VehicleProposal>()
            .With(x => x.Manufacturer, new ChangeRequest<string>(manufacturer!))
            .Create();
        var validatorInput = new CreateVehicleInput(proposal);

        // Act
        var result = await _validator.TestValidateAsync(validatorInput);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(x => x.ErrorCode == ErrorCodes.Vehicle.MandatoryVehicleManufacturer.Code);
        result.Errors.Should()
            .Contain(x => x.ErrorMessage == ErrorCodes.Vehicle.MandatoryVehicleManufacturer.ErrorMessage);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async void OnValidate_GivenACreateVehicleInput_WhenModelIsNull_ShouldHaveValidationErrors(
        string? model)
    {
        var proposal = _fixture.Build<VehicleProposal>()
            .With(x => x.Model, new ChangeRequest<string>(model!))
            .Create();
        var validatorInput = new CreateVehicleInput(proposal);

        // Act
        var result = await _validator.TestValidateAsync(validatorInput);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(x => x.ErrorCode == ErrorCodes.Vehicle.MandatoryVehicleModel.Code);
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorCodes.Vehicle.MandatoryVehicleModel.ErrorMessage);
    }

    [Fact]
    public async void OnValidate_GivenACreateVehicleInput_WhenYearIsLowerThenLimit_ShouldHaveValidationErrors()
    {
        var proposal = _fixture.Build<VehicleProposal>()
            .With(x => x.Year, new ChangeRequest<int>(_random.Next(0)))
            .Create();
        var validatorInput = new CreateVehicleInput(proposal);

        // Act
        var result = await _validator.TestValidateAsync(validatorInput);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(x => x.ErrorCode == ErrorCodes.Vehicle.InvalidVehicleYear.Code);
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorCodes.Vehicle.InvalidVehicleYear.ErrorMessage);
    }

    [Fact]
    public async void OnValidate_GivenACreateVehicleInput_WhenStartingBidIsInvalid_ShouldHaveValidationErrors()
    {
        var proposal = _fixture.Build<VehicleProposal>()
            .With(x => x.StartingBid, new ChangeRequest<decimal>(_random.Next(0)))
            .Create();
        var validatorInput = new CreateVehicleInput(proposal);

        // Act
        var result = await _validator.TestValidateAsync(validatorInput);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(x => x.ErrorCode == ErrorCodes.Vehicle.InvalidVehicleStartingBid.Code);
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorCodes.Vehicle.InvalidVehicleStartingBid.ErrorMessage);
    }

    [Fact]
    public async void OnValidate_GivenACreateVehicleInput_WhenDoorsNumberIsInvalid_ShouldHaveValidationErrors()
    {
        var proposal = _fixture.Build<VehicleProposal>()
            .With(x => x.VehicleType, new ChangeRequest<VehicleType>(VehicleType.Sedan))
            .With(x => x.Year, new ChangeRequest<int>(_random.Next(1885, DateTime.UtcNow.Year)))
            .With(x => x.DoorsNumber, new ChangeRequest<int>(_random.Next(2)))
            .Create();
        var validatorInput = new CreateVehicleInput(proposal);

        // Act
        var result = await _validator.TestValidateAsync(validatorInput);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(x => x.ErrorCode == ErrorCodes.Vehicle.InvalidVehicleDoorsNumber.Code);
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorCodes.Vehicle.InvalidVehicleDoorsNumber.ErrorMessage);
    }

    [Fact]
    public async void OnValidate_GivenACreateVehicleInput_WhenSeatsNumberIsInvalid_ShouldHaveValidationErrors()
    {
        var proposal = _fixture.Build<VehicleProposal>()
            .With(x => x.VehicleType, new ChangeRequest<VehicleType>(VehicleType.Suv))
            .With(x => x.Year, new ChangeRequest<int>(_random.Next(1885, DateTime.UtcNow.Year)))
            .With(x => x.SeatsNumber, new ChangeRequest<int>(_random.Next(0)))
            .Create();
        var validatorInput = new CreateVehicleInput(proposal);

        // Act
        var result = await _validator.TestValidateAsync(validatorInput);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(x => x.ErrorCode == ErrorCodes.Vehicle.InvalidVehicleSeatsNumber.Code);
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorCodes.Vehicle.InvalidVehicleSeatsNumber.ErrorMessage);
    }

    [Fact]
    public async void OnValidate_GivenACreateVehicleInput_WhenLoadCapacityIsInvalid_ShouldHaveValidationErrors()
    {
        var proposal = _fixture.Build<VehicleProposal>()
            .With(x => x.VehicleType, new ChangeRequest<VehicleType>(VehicleType.Truck))
            .With(x => x.Year, new ChangeRequest<int>(_random.Next(1885, DateTime.UtcNow.Year)))
            .With(x => x.LoadCapacity, new ChangeRequest<int>(_random.Next(0)))
            .Create();
        var validatorInput = new CreateVehicleInput(proposal);

        // Act
        var result = await _validator.TestValidateAsync(validatorInput);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(x => x.ErrorCode == ErrorCodes.Vehicle.InvalidVehicleLoadCapacity.Code);
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorCodes.Vehicle.InvalidVehicleLoadCapacity.ErrorMessage);
    }
}