namespace Car.Auction.Management.System.Application.Factories;

using Car.Auction.Management.System.Contracts.Web.Vehicle;
using Car.Auction.Management.System.Models.Aggregates.Vehicle;
using Car.Auction.Management.System.Models.ErrorCodes;
using Car.Auction.Management.System.Models.Exceptions.Vehicle;

public static class VehicleFactory
{
    public static Vehicle CreateVehicle(VehicleProposal proposal)
        => proposal.VehicleType.Value switch
        {
            VehicleType.Hatchback => new Hatchback(proposal),
            VehicleType.Sedan => new Sedan(proposal),
            VehicleType.Suv => new Suv(proposal),
            VehicleType.Truck => new Truck(proposal),
            _ => throw new InvalidVehicleTypeException(
                ErrorCodes.Vehicle.InvalidVehicleType.Code,
                ErrorCodes.Vehicle.InvalidVehicleType.ErrorMessage),
        };
}