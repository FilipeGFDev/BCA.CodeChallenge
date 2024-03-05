namespace Car.Auction.Management.System.Application.Helpers;

using Car.Auction.Management.System.Models.Aggregates.Auction;

public static class AuctionHelper
{
    public static bool IsIsActiveFieldOperation(string path)
        => string.Equals(
            path,
            $"/{nameof(Auction.IsActive)}",
            StringComparison.InvariantCultureIgnoreCase);
}