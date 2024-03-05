namespace Car.Auction.Management.System.Models.Aggregates.Bid;

using Car.Auction.Management.System.Models.Aggregates.Auction;

public class Bid
{
    public Guid Id { get; }

    public Guid UserId { get; }

    public decimal Amount { get; }

    public Guid AuctionId { get; }

    public Auction Auction { get; }

    public DateTime CreatedAt { get; }

    public Bid(BidProposal proposal)
    {
        Id = Guid.NewGuid();
        UserId = proposal.UserId.Value;
        Amount = proposal.Amount.Value;
        AuctionId = proposal.AuctionId.Value;
        CreatedAt = DateTime.UtcNow;
    }

    public Bid() { }
}