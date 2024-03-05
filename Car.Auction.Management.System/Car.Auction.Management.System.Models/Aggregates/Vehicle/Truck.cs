namespace Car.Auction.Management.System.Models.Aggregates.Vehicle;

public class Truck : Vehicle
{
    public int LoadCapacity { get; }

    public Truck(VehicleProposal proposal) : base(proposal)
    {
        LoadCapacity = proposal.LoadCapacity.Value;
    }

    public Truck() { }
}