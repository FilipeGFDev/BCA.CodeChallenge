namespace Car.Auction.Management.System.Application.UseCases.Bid.Create;

using Car.Auction.Management.System.Models.Aggregates.Auction;
using Car.Auction.Management.System.Models.Aggregates.Bid;
using Car.Auction.Management.System.Models.Core;
using Car.Auction.Management.System.Models.ErrorCodes;
using FluentValidation;
using FluentValidation.Results;
using global::System.Linq.Expressions;

public class CreateBidInputValidator : AbstractValidator<CreateBidInput>
{
    private readonly IRepository<Bid> _bidRepository;
    private readonly IRepository<Auction> _auctionRepository;

    public CreateBidInputValidator(
        IRepository<Bid> bidRepository,
        IRepository<Auction> auctionRepository)
    {
        _bidRepository = bidRepository;
        _auctionRepository = auctionRepository;

        RuleFor(x => x.Proposal.UserId)
            .Must(x => x.IsSet)
            .WithErrorCode(ErrorCodes.Bid.MandatoryBidUserId.Code)
            .WithMessage(ErrorCodes.Bid.MandatoryBidUserId.ErrorMessage)
            .DependentRules(
                () => RuleFor(x => x.Proposal.UserId.Value)
                    .Must(x => x != Guid.Empty)
                    .WithErrorCode(ErrorCodes.Bid.InvalidBidUserId.Code)
                    .WithMessage(ErrorCodes.Bid.InvalidBidUserId.ErrorMessage));

        RuleFor(x => x.Proposal.AuctionId)
            .Must(x => x.IsSet)
            .WithErrorCode(ErrorCodes.Bid.MandatoryBidAuctionId.Code)
            .WithMessage(ErrorCodes.Bid.MandatoryBidAuctionId.ErrorMessage)
            .DependentRules(() =>
                RuleFor(x => x.Proposal)
                    .CustomAsync(ValidateAuctionAsync));
    }

    private async Task ValidateAuctionAsync(
        BidProposal proposal,
        ValidationContext<CreateBidInput> context,
        CancellationToken cancellationToken)
    {
        var auction = await _auctionRepository.Get(proposal.AuctionId.Value, cancellationToken);

        if (auction is null)
        {
            context.AddFailure(
                new ValidationFailure(
                    string.Empty,
                    ErrorCodes.Bid.BidAuctionNotFound.ErrorMessage,
                    proposal.AuctionId.Value)
                {
                    ErrorCode = ErrorCodes.Bid.BidAuctionNotFound.Code,
                    CustomState = $"AuctionId: \"{proposal.AuctionId.Value}\"",
                });

            return;
        }

        if (!auction.IsActive)
        {
            context.AddFailure(
                new ValidationFailure(
                    string.Empty,
                    ErrorCodes.Bid.BidAuctionUnavailable.ErrorMessage,
                    proposal.AuctionId.Value)
                {
                    ErrorCode = ErrorCodes.Bid.BidAuctionUnavailable.Code,
                    CustomState = $"AuctionId: \"{proposal.AuctionId.Value}\"",
                });

            return;
        }

        var highestBidsResult = await _bidRepository.Get(
            new(1, 1),
            new List<Expression<Func<Bid, bool>>> { x => x.AuctionId == proposal.AuctionId.Value },
            x => x.OrderByDescending(b => b.Amount),
            cancellationToken);

        var highestBid = highestBidsResult.Entries.FirstOrDefault();

        if (highestBid is not null && highestBid.Amount >= proposal.Amount.Value)
        {
            context.AddFailure(
                new ValidationFailure(
                    string.Empty,
                    ErrorCodes.Bid.AmountSmallerThanCurrentBid.ErrorMessage,
                    proposal.AuctionId.Value)
                {
                    ErrorCode = ErrorCodes.Bid.AmountSmallerThanCurrentBid.Code,
                    CustomState =
                        $"AuctionId: \"{proposal.AuctionId.Value}\", CurrentBid: \"{highestBid.Amount}\", Bid: \"{proposal.Amount.Value}\"",
                });

            return;
        }

        if (highestBid is null && proposal.Amount.Value < auction.Vehicle.StartingBid)
        {
            context.AddFailure(
                new ValidationFailure(
                    string.Empty,
                    ErrorCodes.Bid.AmountSmallerThanVehicleStaringBid.ErrorMessage,
                    proposal.AuctionId.Value)
                {
                    ErrorCode = ErrorCodes.Bid.AmountSmallerThanVehicleStaringBid.Code,
                    CustomState =
                        $"AuctionId: \"{proposal.AuctionId.Value}\", VehicleStartingBid: \"{auction.Vehicle.StartingBid}\", Bid: \"{proposal.Amount.Value}\"",
                });
        }
    }
}