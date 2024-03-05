namespace Car.Auction.Management.System.Application.Tests.UseCases.Bid.Create;

using AutoFixture;
using FluentAssertions;
using FluentValidation.TestHelper;
using Car.Auction.Management.System.Application.UseCases.Bid.Create;
using Car.Auction.Management.System.Contracts.Web.Paged;
using Car.Auction.Management.System.Models.Aggregates.Auction;
using Car.Auction.Management.System.Models.Aggregates.Bid;
using Car.Auction.Management.System.Models.Core;
using Car.Auction.Management.System.Models.ErrorCodes;
using Moq;
using Xunit;
using global::System.Linq.Expressions;

public class CreateBidInputValidatorTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IRepository<Bid>> _bidRepositoryMock;
    private readonly Mock<IRepository<Auction>> _auctionRepositoryMock;
    private readonly CreateBidInputValidator _validator;

    public CreateBidInputValidatorTests()
    {
        _fixture = new Fixture();
        _bidRepositoryMock = new();
        _auctionRepositoryMock = new();
        _validator = new(_bidRepositoryMock.Object, _auctionRepositoryMock.Object);
    }

    [Fact]
    public async void OnValidate_GivenACreateBidInput_WhenBidAmountIsBiggerThenCurrent_ShouldNotHaveValidationErrors()
    {
        var proposal = _fixture.Build<BidProposal>()
            .With(x => x.Amount, 10)
            .Create();
        var validatorInput = new CreateBidInput(proposal);

        var auction = new Auction(_fixture.Create<AuctionProposal>());
        var highestBid = new PagedQueryResult<Bid>(1, new[]
        {
            new Bid(_fixture.Build<BidProposal>()
                .With(x => x.AuctionId, auction.Id)
                .With(x => x.Amount, 1)
                .Create())
        });

        _auctionRepositoryMock.Setup(x =>
                x.Get(validatorInput.Proposal.AuctionId.Value, It.IsAny<CancellationToken>()))
            .ReturnsAsync(auction);

        _bidRepositoryMock.Setup(x =>
                x.Get(
                    It.IsAny<PageInformation>(),
                    It.IsAny<List<Expression<Func<Bid, bool>>>>(),
                    It.IsAny<Func<IQueryable<Bid>, IOrderedQueryable<Bid>>?>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(highestBid);

        // Act
        var result = await _validator.TestValidateAsync(validatorInput);

        // Assert
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public async void OnValidate_GivenACreateBidInput_WhenUserIdIsNotSet_ShouldHaveValidationErrors()
    {
        var proposal = _fixture.Build<BidProposal>()
            .Without(x => x.UserId)
            .Create();
        var validatorInput = new CreateBidInput(proposal);

        // Act
        var result = await _validator.TestValidateAsync(validatorInput);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(x => x.ErrorCode == ErrorCodes.Bid.MandatoryBidUserId.Code);
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorCodes.Bid.MandatoryBidUserId.ErrorMessage);
    }

    [Fact]
    public async void OnValidate_GivenACreateBidInput_WhenUserIdIsEmpty_ShouldHaveValidationErrors()
    {
        var proposal = _fixture.Build<BidProposal>()
            .With(x => x.UserId, Guid.Empty)
            .Create();
        var validatorInput = new CreateBidInput(proposal);

        // Act
        var result = await _validator.TestValidateAsync(validatorInput);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(x => x.ErrorCode == ErrorCodes.Bid.InvalidBidUserId.Code);
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorCodes.Bid.InvalidBidUserId.ErrorMessage);
    }

    [Fact]
    public async void OnValidate_GivenACreateBidInput_WhenAuctionIdIsNotSet_ShouldHaveValidationErrors()
    {
        var proposal = _fixture.Build<BidProposal>()
            .Without(x => x.AuctionId)
            .Create();
        var validatorInput = new CreateBidInput(proposal);

        // Act
        var result = await _validator.TestValidateAsync(validatorInput);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(x => x.ErrorCode == ErrorCodes.Bid.MandatoryBidAuctionId.Code);
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorCodes.Bid.MandatoryBidAuctionId.ErrorMessage);
    }

    [Fact]
    public async void OnValidate_GivenACreateBidInput_WhenAuctionIdDoesNotExist_ShouldHaveValidationErrors()
    {
        var proposal = _fixture.Create<BidProposal>();
        var validatorInput = new CreateBidInput(proposal);

        _auctionRepositoryMock.Setup(x =>
                x.Get(validatorInput.Proposal.AuctionId.Value, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Auction)null!);

        // Act
        var result = await _validator.TestValidateAsync(validatorInput);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(x => x.ErrorCode == ErrorCodes.Bid.BidAuctionNotFound.Code);
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorCodes.Bid.BidAuctionNotFound.ErrorMessage);
    }

    [Fact]
    public async void OnValidate_GivenACreateBidInput_WhenAuctionIsNotActive_ShouldHaveValidationErrors()
    {
        var proposal = _fixture.Create<BidProposal>();
        var validatorInput = new CreateBidInput(proposal);

        var auction = new Auction(_fixture.Create<AuctionProposal>());
        auction.Close();

        _auctionRepositoryMock.Setup(x =>
                x.Get(validatorInput.Proposal.AuctionId.Value, It.IsAny<CancellationToken>()))
            .ReturnsAsync(auction);

        // Act
        var result = await _validator.TestValidateAsync(validatorInput);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(x => x.ErrorCode == ErrorCodes.Bid.BidAuctionUnavailable.Code);
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorCodes.Bid.BidAuctionUnavailable.ErrorMessage);
    }

    [Fact]
    public async void OnValidate_GivenACreateBidInput_WhenBidAmountIsSmallerThenCurrent_ShouldHaveValidationErrors()
    {
        var proposal = _fixture.Build<BidProposal>()
            .With(x => x.Amount, 1)
            .Create();
        var validatorInput = new CreateBidInput(proposal);

        var auction = new Auction(_fixture.Create<AuctionProposal>());
        var highestBid = new PagedQueryResult<Bid>(1, new[]
        {
            new Bid(_fixture.Build<BidProposal>()
                .With(x => x.AuctionId, auction.Id)
                .With(x => x.Amount, 2)
                .Create())
        });

        _auctionRepositoryMock.Setup(x =>
                x.Get(validatorInput.Proposal.AuctionId.Value, It.IsAny<CancellationToken>()))
            .ReturnsAsync(auction);

        _bidRepositoryMock.Setup(x =>
                x.Get(
                    It.IsAny<PageInformation>(),
                    It.IsAny<List<Expression<Func<Bid, bool>>>>(),
                    It.IsAny<Func<IQueryable<Bid>, IOrderedQueryable<Bid>>?>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(highestBid);

        // Act
        var result = await _validator.TestValidateAsync(validatorInput);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(x => x.ErrorCode == ErrorCodes.Bid.AmountSmallerThanCurrentBid.Code);
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorCodes.Bid.AmountSmallerThanCurrentBid.ErrorMessage);
    }
}