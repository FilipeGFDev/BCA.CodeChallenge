namespace Car.Auction.Management.System.Web.Mappings.Bid;

using AutoMapper;
using Car.Auction.Management.System.Application.UseCases.Bid.Create;
using Car.Auction.Management.System.Contracts.Web.Bid;
using Car.Auction.Management.System.Models.Aggregates.Bid;

public class BidRequestMapping : Profile
{
    public BidRequestMapping()
    {
        CreateMap<BidRequest, CreateBidInput>()
            .ConvertUsing(
                request =>
                    new(
                        new()
                        {
                            Amount = request.Amount,
                            AuctionId = request.AuctionId,
                            UserId = request.UserId,
                        }));
    }
}