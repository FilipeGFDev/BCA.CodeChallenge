namespace Car.Auction.Management.System.Models.Aggregates.Bid;

using Car.Auction.Management.System.Models.Core;

public record BidProposal
{
    public ChangeRequest<Guid> UserId { get; set; }

    public ChangeRequest<decimal> Amount { get; set; }

    public ChangeRequest<Guid> AuctionId { get; set; }
}