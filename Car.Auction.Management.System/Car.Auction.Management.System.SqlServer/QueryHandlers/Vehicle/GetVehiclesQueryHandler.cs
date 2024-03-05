namespace Car.Auction.Management.System.SqlServer.QueryHandlers.Vehicle;

using AutoMapper;
using Car.Auction.Management.System.Contracts.Web.Paged;
using Car.Auction.Management.System.Contracts.Web.Vehicle;
using Car.Auction.Management.System.Contracts.Web.Vehicle.Get;
using Car.Auction.Management.System.Models.Aggregates.Vehicle;
using Car.Auction.Management.System.Models.Core;
using MediatR;
using global::System.Linq.Expressions;

public class GetVehiclesQueryHandler : IRequestHandler<GetVehiclesRequest, PagedResult<GetVehicleResponse>>
{
    private readonly IRepository<Vehicle> _repository;
    private readonly IMapper _mapper;

    public GetVehiclesQueryHandler(
        IRepository<Vehicle> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedResult<GetVehicleResponse>> Handle(
        GetVehiclesRequest request,
        CancellationToken cancellationToken)
    {
        var pageInformation = new PageInformation(request.Filter);

        var filters = new List<Expression<Func<Vehicle, bool>>>();

        if (request.Filter.VehicleType is not null && request.Filter.VehicleType is not VehicleType.None)
        {
            var vehicleType = Enum.GetName(typeof(VehicleType), request.Filter.VehicleType);
            filters.Add(x => x.VehicleType == vehicleType);
        }
        
        if (!string.IsNullOrWhiteSpace(request.Filter.Manufacturer))
        {
            filters.Add(x => x.Manufacturer == request.Filter.Manufacturer);
        }

        if (!string.IsNullOrWhiteSpace(request.Filter.Model))
        {
            filters.Add(x => x.Model == request.Filter.Model);
        }

        if (request.Filter.Year is not null)
        {
            filters.Add(x => x.Year == request.Filter.Year);
        }

        if (request.Filter.IsAvailable is not null)
        {
            filters.Add(x => x.IsAvailable == request.Filter.IsAvailable);
        }

        var result = await _repository.Get(
            new(request.Filter),
            filters,
            null,
            cancellationToken);

        return new(
            pageInformation,
            result.TotalItems,
            _mapper.Map<IEnumerable<GetVehicleResponse>>(result.Entries));
    }
}