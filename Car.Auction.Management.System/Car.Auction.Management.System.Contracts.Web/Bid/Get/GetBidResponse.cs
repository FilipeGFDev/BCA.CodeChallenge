namespace Car.Auction.Management.System.Contracts.Web.Bid.Get;

public record GetBidResponse(
    Guid Id,
    Guid UserId,
    decimal Amount,
    DateTime CreatedAt);