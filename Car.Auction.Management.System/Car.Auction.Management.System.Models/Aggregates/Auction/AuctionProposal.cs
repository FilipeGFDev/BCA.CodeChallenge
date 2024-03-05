namespace Car.Auction.Management.System.Models.Aggregates.Auction;

using Car.Auction.Management.System.Models.Core;

public record AuctionProposal
{
    public ChangeRequest<string?> Description { get; set; }

    public ChangeRequest<Guid?> VehicleId { get; set; }
};