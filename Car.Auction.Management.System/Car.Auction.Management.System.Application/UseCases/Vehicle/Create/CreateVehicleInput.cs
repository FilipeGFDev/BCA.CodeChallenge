namespace Car.Auction.Management.System.Application.UseCases.Vehicle.Create;

using Car.Auction.Management.System.Application.Core;
using Car.Auction.Management.System.Models.Aggregates.Vehicle;

public record CreateVehicleInput(VehicleProposal Proposal) : IUseCaseInput<VehicleCreatedUseCaseEvent>;