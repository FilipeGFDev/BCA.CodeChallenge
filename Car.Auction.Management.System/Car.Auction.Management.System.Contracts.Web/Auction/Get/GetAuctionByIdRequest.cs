namespace Car.Auction.Management.System.Contracts.Web.Auction.Get;

using MediatR;

public record GetAuctionByIdRequest(Guid AuctionId) : IRequest<GetAuctionResponse>;