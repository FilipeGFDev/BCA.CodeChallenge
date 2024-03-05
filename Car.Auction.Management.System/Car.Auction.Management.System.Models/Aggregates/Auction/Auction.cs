namespace Car.Auction.Management.System.Models.Aggregates.Auction;

using Car.Auction.Management.System.Models.Aggregates.Bid;
using Car.Auction.Management.System.Models.Aggregates.Vehicle;

public class Auction
{
    public Guid Id { get; }

    public string Description { get; }

    public Guid? VehicleId { get; }

    public Vehicle Vehicle { get; }

    public ICollection<Bid> Bids { get; }

    public DateTime StartedAt { get; }

    public DateTime ClosedAt { get; private set; }

    public DateTime CreatedAt { get; }
    
    public bool IsActive { get; private set; }

    public Auction(AuctionProposal proposal)
    {
        Id = Guid.NewGuid();
        Description = proposal.Description.Value;
        VehicleId = proposal.VehicleId.Value;
        Bids = new List<Bid>();
        StartedAt = DateTime.UtcNow;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    public Auction() { }

    public void Close()
    {
        IsActive = false;
        ClosedAt = DateTime.UtcNow;
    }
}