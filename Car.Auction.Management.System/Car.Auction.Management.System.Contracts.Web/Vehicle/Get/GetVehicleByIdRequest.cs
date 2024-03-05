namespace Car.Auction.Management.System.Contracts.Web.Vehicle.Get;

using MediatR;

public record GetVehicleByIdRequest(Guid VehicleId) : IRequest<GetVehicleResponse>;