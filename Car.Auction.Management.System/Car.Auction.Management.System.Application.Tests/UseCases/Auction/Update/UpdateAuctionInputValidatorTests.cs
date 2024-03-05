namespace Car.Auction.Management.System.Application.Tests.UseCases.Auction.Update;

using AutoFixture;
using Car.Auction.Management.System.Application.UseCases.Auction.Update;
using Car.Auction.Management.System.Models.Aggregates.Auction;
using Car.Auction.Management.System.Models.ErrorCodes;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.JsonPatch;
using Xunit;

public class UpdateAuctionInputValidatorTests
{
    private readonly IFixture _fixture;
    private readonly UpdateAuctionInputValidator _validator;

    public UpdateAuctionInputValidatorTests()
    {
        _fixture = new Fixture();
        _validator = new UpdateAuctionInputValidator();
    }

    [Fact]
    public async void OnValidate_GivenAUpdateAuctionInput_WhenOperationsAreAValid_ShouldNotHaveValidationErrors()
    {
        var id = _fixture.Create<Guid>();
        var document = new JsonPatchDocument<Auction>();
        document.Replace(x => x.IsActive, false);
        var validatorInput = new UpdateAuctionInput(id, document);

        // Act
        var result = await _validator.TestValidateAsync(validatorInput);

        // Assert
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public async void OnValidate_GivenAUpdateAuctionInput_WhenContainsOperationsNotReplace_ShouldHaveValidationErrors()
    {
        var id = _fixture.Create<Guid>();
        var document = new JsonPatchDocument<Auction>();
        document.Replace(x => x.IsActive, false);
        document.Remove(x => x.Description);
        var validatorInput = new UpdateAuctionInput(id, document);

        // Act
        var result = await _validator.TestValidateAsync(validatorInput);

        // Assert
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().Contain(x =>
            x.ErrorCode == ErrorCodes.Application.InvalidJsonPatchOperationType.Code);
        result.Errors.Should().Contain(x =>
            x.ErrorMessage == ErrorCodes.Application.InvalidJsonPatchOperationType.ErrorMessage);
    }

    [Fact]
    public async void OnValidate_GivenAUpdateAuctionInput_WhenOperationIsNotForIsActiveField_ShouldHaveValidationErrors()
    {
        var id = _fixture.Create<Guid>();
        var document = new JsonPatchDocument<Auction>();
        document.Replace(x => x.Description, _fixture.Create<string>());
        var validatorInput = new UpdateAuctionInput(id, document);

        // Act
        var result = await _validator.TestValidateAsync(validatorInput);

        // Assert
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().Contain(x => x.ErrorCode == ErrorCodes.Auction.InvalidAuctionFieldUpdate.Code);
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorCodes.Auction.InvalidAuctionFieldUpdate.ErrorMessage);
    }

    [Fact]
    public async void OnValidate_GivenAUpdateAuctionInput_WhenOperationIsForIsActiveField_WithValueTrue_ShouldHaveValidationErrors()
    {
        var id = _fixture.Create<Guid>();
        var document = new JsonPatchDocument<Auction>();
        document.Replace(x => x.IsActive, true);
        var validatorInput = new UpdateAuctionInput(id, document);

        // Act
        var result = await _validator.TestValidateAsync(validatorInput);

        // Assert
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().Contain(x => x.ErrorCode == ErrorCodes.Auction.InvalidAuctionIsActiveFieldUpdate.Code);
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorCodes.Auction.InvalidAuctionIsActiveFieldUpdate.ErrorMessage);
    }
}