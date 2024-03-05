namespace Car.Auction.Management.System.Web.Mappings.Vehicle;

using AutoMapper;
using Car.Auction.Management.System.Application.UseCases.Vehicle.Create;
using Car.Auction.Management.System.Contracts.Web.Vehicle;
using Car.Auction.Management.System.Models.Aggregates.Vehicle;

public class VehicleRequestMapping : Profile
{
    public VehicleRequestMapping()
    {
        CreateMap<VehicleRequest, CreateVehicleInput>()
            .ConvertUsing(
                request =>
                    new(
                        new()
                        {
                            VehicleType = request.VehicleType ?? default,
                            Manufacturer = request.Manufacturer,
                            Model = request.Model,
                            Year = request.Year,
                            StartingBid = request.StartingBid,
                            DoorsNumber = request.DoorsNumber ?? default,
                            LoadCapacity = request.LoadCapacity ?? default,
                            SeatsNumber = request.SeatsNumber ?? default,
                        }));
    }
}