namespace Car.Auction.Management.System.SqlServer.Mappings.Auction;

using AutoMapper;
using Car.Auction.Management.System.Contracts.Web.Auction.Get;
using Car.Auction.Management.System.Contracts.Web.Bid.Get;
using Car.Auction.Management.System.Contracts.Web.Vehicle.Get;
using Car.Auction.Management.System.Models.Aggregates.Auction;

public class AuctionMapping : Profile
{
    public AuctionMapping()
    {
        CreateMap<Auction, GetAuctionResponse>()
            .ConvertUsing(
                (source, _, ctx) => new(
                    source.Id,
                    source.Description,
                    source.StartedAt,
                    source.ClosedAt,
                    source.IsActive,
                    ctx.Mapper.Map<GetVehicleResponse>(source.Vehicle),
                    ctx.Mapper.Map<List<GetBidResponse>>(source.Bids)));
    }
}