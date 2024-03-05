namespace Car.Auction.Management.System.Contracts.Web.Bid;

public class BidRequest
{
    public Guid UserId { get; set; }

    public decimal Amount { get; set; }

    public Guid AuctionId { get; set; }
}