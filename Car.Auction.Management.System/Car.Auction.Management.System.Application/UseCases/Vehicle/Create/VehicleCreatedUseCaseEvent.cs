namespace Car.Auction.Management.System.Application.UseCases.Vehicle.Create;

using Car.Auction.Management.System.Application.Core;

public record VehicleCreatedUseCaseEvent(Guid VehicleId) : IUseCaseEvent;
