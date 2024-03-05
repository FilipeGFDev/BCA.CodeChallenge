namespace Car.Auction.Management.System.Models.Aggregates.Vehicle;

public abstract class Vehicle
{
    public Guid Id { get; }

    public string Manufacturer { get; }

    public string Model { get; }

    public int Year { get; }

    public decimal StartingBid { get; }

    public bool IsAvailable { get; set; }

    public string VehicleType { get; set; }

    public DateTime CreatedAt { get; }

    protected Vehicle(VehicleProposal proposal)
    {
        Id = Guid.NewGuid();
        Manufacturer = proposal.Manufacturer.Value;
        Model = proposal.Model.Value;
        Year = proposal.Year.Value;
        StartingBid = proposal.StartingBid.Value;
        IsAvailable = true;
        CreatedAt = DateTime.UtcNow;
    }

    protected Vehicle() { }
    
    public void ChangeToUnavailable()
    {
        IsAvailable = false;
    }
}