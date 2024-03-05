namespace Car.Auction.Management.System.Models.Aggregates.Vehicle;

public class Suv : Vehicle
{
    public int SeatsNumber { get; }

    public Suv(VehicleProposal proposal) : base(proposal)
    {
        SeatsNumber = proposal.SeatsNumber.Value;
    }

    public Suv() { }
}