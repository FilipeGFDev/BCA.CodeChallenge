namespace Car.Auction.Management.System.Contracts.Web.Auction;

public class AuctionRequest
{
    public string Description { get; set; }

    public Guid VehicleId { get; set; }
}