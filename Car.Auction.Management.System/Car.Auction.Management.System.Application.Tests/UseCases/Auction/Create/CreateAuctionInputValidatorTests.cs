namespace Car.Auction.Management.System.Application.Tests.UseCases.Auction.Create;

using AutoFixture;
using Car.Auction.Management.System.Application.UseCases.Auction.Create;
using Car.Auction.Management.System.Contracts.Web.Paged;
using Car.Auction.Management.System.Models.Aggregates.Auction;
using Car.Auction.Management.System.Models.Aggregates.Vehicle;
using Car.Auction.Management.System.Models.Core;
using Car.Auction.Management.System.Models.ErrorCodes;
using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using Xunit;
using global::System.Linq.Expressions;

public class CreateAuctionInputValidatorTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IRepository<Auction>> _auctionRepositoryMock;
    private readonly Mock<IRepository<Vehicle>> _vehicleRepositoryMock;
    private readonly CreateAuctionInputValidator _validator;

    public CreateAuctionInputValidatorTests()
    {
        _fixture = new Fixture();
        _auctionRepositoryMock = new Mock<IRepository<Auction>>();
        _vehicleRepositoryMock = new Mock<IRepository<Vehicle>>();
        _validator = new CreateAuctionInputValidator(
            _auctionRepositoryMock.Object,
            _vehicleRepositoryMock.Object);
    }

    [Fact]
    public async void OnValidate_GivenACreateAuctionInput_WhenVehicleIsAValid_ShouldNotHaveValidationErrors()
    {
        var vehicle = new Hatchback(_fixture.Create<VehicleProposal>());
        var proposal = _fixture.Build<AuctionProposal>()
            .With(x => x.VehicleId, vehicle.Id)
            .Create();
        var validatorInput = new CreateAuctionInput(proposal);

        var pagedResult = new PagedQueryResult<Auction>(0, new List<Auction>());

        _vehicleRepositoryMock
            .Setup(x => x.Get(vehicle.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        _auctionRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<PageInformation>(),
                It.IsAny<List<Expression<Func<Auction, bool>>>>(),
                null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _validator.TestValidateAsync(validatorInput);

        // Assert
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public async void OnValidate_GivenACreateAuctionInput_WhenVehicleIdIsNotSet_ShouldHaveValidationErrors()
    {
        var proposal = _fixture.Build<AuctionProposal>()
            .Without(x => x.VehicleId)
            .Create();
        var validatorInput = new CreateAuctionInput(proposal);

        // Act
        var result = await _validator.TestValidateAsync(validatorInput);

        // Assert
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().Contain(x => x.ErrorCode == ErrorCodes.Auction.MandatoryAuctionVehicleId.Code);
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorCodes.Auction.MandatoryAuctionVehicleId.ErrorMessage);
    }

    [Fact]
    public async void OnValidate_GivenACreateAuctionInput_WhenVehicleIdIsEmpty_ShouldHaveValidationErrors()
    {
        var proposal = _fixture.Build<AuctionProposal>()
            .With(x => x.VehicleId, Guid.Empty)
            .Create();
        var validatorInput = new CreateAuctionInput(proposal);

        // Act
        var result = await _validator.TestValidateAsync(validatorInput);

        // Assert
        result.Errors.Should().HaveCount(1);
        result.ShouldHaveValidationErrorFor(x => x.Proposal.VehicleId.Value)
            .WithErrorCode(ErrorCodes.Auction.InvalidAuctionVehicleId.Code)
            .WithErrorMessage(ErrorCodes.Auction.InvalidAuctionVehicleId.ErrorMessage);
    }

    [Fact]
    public async void OnValidate_GivenACreateAuctionInput_WhenVehicleDoesntExist_ShouldHaveValidationErrors()
    {
        var vehicleId = _fixture.Create<Guid>();
        var proposal = _fixture.Build<AuctionProposal>()
            .With(x => x.VehicleId, vehicleId)
            .Create();
        var validatorInput = new CreateAuctionInput(proposal);

        _vehicleRepositoryMock
            .Setup(x => x.Get(vehicleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Vehicle)null!);

        // Act
        var result = await _validator.TestValidateAsync(validatorInput);

        // Assert
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().Contain(x => x.ErrorCode == ErrorCodes.Auction.AuctionVehicleNotFound.Code);
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorCodes.Auction.AuctionVehicleNotFound.ErrorMessage);
    }

    [Fact]
    public async void OnValidate_GivenACreateAuctionInput_WhenVehicleIsNotAvailable_ShouldHaveValidationErrors()
    {
        var vehicle = new Hatchback(_fixture.Create<VehicleProposal>());
        vehicle.ChangeToUnavailable();
        var proposal = _fixture.Build<AuctionProposal>()
            .With(x => x.VehicleId, vehicle.Id)
            .Create();
        var validatorInput = new CreateAuctionInput(proposal);

        _vehicleRepositoryMock
            .Setup(x => x.Get(vehicle.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        // Act
        var result = await _validator.TestValidateAsync(validatorInput);

        // Assert
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().Contain(x => x.ErrorCode == ErrorCodes.Auction.AuctionVehicleIsUnavailable.Code);
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorCodes.Auction.AuctionVehicleIsUnavailable.ErrorMessage);
    }

    [Fact]
    public async void OnValidate_GivenACreateAuctionInput_WhenVehicleIsAlreadyOnAuction_ShouldHaveValidationErrors()
    {
        var vehicle = new Hatchback(_fixture.Create<VehicleProposal>());
        var proposal = _fixture.Build<AuctionProposal>()
            .With(x => x.VehicleId, vehicle.Id)
            .Create();
        var validatorInput = new CreateAuctionInput(proposal);

        var pagedResult = new PagedQueryResult<Auction>(1, new List<Auction> { _fixture.Create<Auction>() });

        _vehicleRepositoryMock
            .Setup(x => x.Get(vehicle.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        _auctionRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<PageInformation>(),
                It.IsAny<List<Expression<Func<Auction, bool>>>>(),
                null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _validator.TestValidateAsync(validatorInput);

        // Assert
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().Contain(x => x.ErrorCode == ErrorCodes.Auction.AuctionVehicleFoundedOnExistentAuction.Code);
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorCodes.Auction.AuctionVehicleFoundedOnExistentAuction.ErrorMessage);
    }
}