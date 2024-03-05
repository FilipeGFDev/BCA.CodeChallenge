namespace Car.Auction.Management.System.Models.Aggregates.Vehicle;

public class Hatchback : Vehicle
{
    public int DoorsNumber { get; set; }

    public Hatchback(VehicleProposal proposal) : base(proposal)
    {
        DoorsNumber = proposal.DoorsNumber.Value;
    }

    public Hatchback() { }
}