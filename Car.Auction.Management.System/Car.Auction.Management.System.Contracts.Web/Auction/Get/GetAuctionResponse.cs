namespace Car.Auction.Management.System.Contracts.Web.Auction.Get;

using Car.Auction.Management.System.Contracts.Web.Bid.Get;
using Car.Auction.Management.System.Contracts.Web.Vehicle.Get;

public record GetAuctionResponse(
    Guid Id,
    string Description,
    DateTime StartedAt,
    DateTime CloseAt,
    bool IsActive,
    GetVehicleResponse Vehicle,
    IEnumerable<GetBidResponse> Bids);