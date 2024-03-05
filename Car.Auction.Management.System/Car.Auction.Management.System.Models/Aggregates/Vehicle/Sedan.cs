namespace Car.Auction.Management.System.Models.Aggregates.Vehicle;

public class Sedan : Vehicle
{
    public int DoorsNumber { get; }

    public Sedan(VehicleProposal proposal) : base(proposal)
    {
        DoorsNumber = proposal.DoorsNumber.Value;
    }

    public Sedan() { }
}