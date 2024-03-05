namespace Car.Auction.Management.System.SqlServer.Mappings.Vehicle;

using AutoMapper;
using Car.Auction.Management.System.Contracts.Web.Vehicle.Get;
using Car.Auction.Management.System.Models.Aggregates.Vehicle;

public class VehicleMapping : Profile
{
    public VehicleMapping()
    {
        CreateMap<Vehicle, GetVehicleResponse>()
            .ConvertUsing<VehicleConverter>();
    }
}