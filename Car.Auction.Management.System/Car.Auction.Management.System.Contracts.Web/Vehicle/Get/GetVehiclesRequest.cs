namespace Car.Auction.Management.System.Contracts.Web.Vehicle.Get;

using Car.Auction.Management.System.Contracts.Web.Paged;
using Car.Auction.Management.System.Contracts.Web.Vehicle;
using MediatR;

public record GetVehiclesRequest(GetVehiclesRequest.RequestFilter Filter)
    : IRequest<PagedResult<GetVehicleResponse>>
{
    public record RequestFilter(
        int? Page,
        int? PageSize,
        VehicleType? VehicleType,
        string? Manufacturer,
        string? Model,
        int? Year,
        bool? IsAvailable) : IPagedRequest;
}