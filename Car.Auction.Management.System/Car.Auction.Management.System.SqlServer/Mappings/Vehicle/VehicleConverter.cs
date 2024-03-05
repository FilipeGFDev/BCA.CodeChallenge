namespace Car.Auction.Management.System.SqlServer.Mappings.Vehicle;

using AutoMapper;
using Car.Auction.Management.System.Contracts.Web.Vehicle;
using Car.Auction.Management.System.Contracts.Web.Vehicle.Get;
using Car.Auction.Management.System.Models.Aggregates.Vehicle;

public class VehicleConverter : ITypeConverter<Vehicle, GetVehicleResponse>
{
    public GetVehicleResponse Convert(
        Vehicle source,
        GetVehicleResponse destination,
        ResolutionContext context)
    {
        var vehicleDataByType = HandleVehicle(source);
        return new(
            source.Id,
            vehicleDataByType.Type,
            source.Manufacturer,
            source.Model,
            source.Year,
            source.StartingBid,
            source.IsAvailable,
            vehicleDataByType.DoorsNumber,
            vehicleDataByType.LoadCapacity,
            vehicleDataByType.SeatsNumber);
    }

    private static (VehicleType Type, int? DoorsNumber, int? LoadCapacity, int? SeatsNumber) HandleVehicle(
        Vehicle source)
        => source switch
        {
            Hatchback hatchback => (VehicleType.Hatchback, hatchback.DoorsNumber, null, null),
            Sedan sedan => (VehicleType.Sedan, sedan.DoorsNumber, null, null),
            Suv suv => (VehicleType.Suv, null, null, suv.SeatsNumber),
            Truck truck => (VehicleType.Truck, null, truck.LoadCapacity, null),
        };
}