namespace Car.Auction.Management.System.Models.Aggregates.Vehicle;

using Car.Auction.Management.System.Contracts.Web.Vehicle;
using Car.Auction.Management.System.Models.Core;

public record VehicleProposal
{
    public ChangeRequest<VehicleType> VehicleType { get; set; }
 
    public ChangeRequest<string> Manufacturer { get; set; }

    public ChangeRequest<string> Model { get; set; }

    public ChangeRequest<int> Year { get; set; }

    public ChangeRequest<decimal> StartingBid { get; set; }

    public ChangeRequest<int> SeatsNumber { get; set; }

    public ChangeRequest<int> LoadCapacity { get; set; }

    public ChangeRequest<int> DoorsNumber { get; set; }
}