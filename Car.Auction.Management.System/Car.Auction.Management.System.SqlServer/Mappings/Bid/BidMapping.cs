namespace Car.Auction.Management.System.SqlServer.Mappings.Bid;

using AutoMapper;
using Car.Auction.Management.System.Contracts.Web.Bid.Get;
using Car.Auction.Management.System.Models.Aggregates.Bid;

public class BidMapping : Profile
{
    public BidMapping()
    {
        CreateMap<Bid, GetBidResponse>()
            .ConvertUsing(
                source => new(
                    source.Id,
                    source.UserId,
                    source.Amount,
                    source.CreatedAt));
    }
}