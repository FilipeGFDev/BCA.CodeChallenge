namespace Car.Auction.Management.System.Contracts.Web.Vehicle.Get;

using Car.Auction.Management.System.Contracts.Web.Vehicle;

public record GetVehicleResponse(
    Guid Id,
    VehicleType VehicleType,
    string Manufacturer,
    string Model,
    int Year,
    decimal StartingBid,
    bool IsAvailable,
    int? DoorsNumber,
    int? LoadCapacity,
    int? SeatsNumber);