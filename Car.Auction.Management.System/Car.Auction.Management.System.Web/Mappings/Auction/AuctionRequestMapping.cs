namespace Car.Auction.Management.System.Web.Mappings.Auction;

using AutoMapper;
using Car.Auction.Management.System.Application.UseCases.Auction.Create;
using Car.Auction.Management.System.Contracts.Web.Auction;
using Car.Auction.Management.System.Models.Aggregates.Auction;

public class AuctionRequestMapping : Profile
{
    public AuctionRequestMapping()
    {
        CreateMap<AuctionRequest, CreateAuctionInput>()
            .ConvertUsing(
                request =>
                    new(
                        new()
                        {
                            Description = request.Description,
                            VehicleId = request.VehicleId,
                        }));
    }
}