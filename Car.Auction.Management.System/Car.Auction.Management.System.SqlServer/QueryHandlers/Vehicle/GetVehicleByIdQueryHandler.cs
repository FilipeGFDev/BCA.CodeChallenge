namespace Car.Auction.Management.System.SqlServer.QueryHandlers.Vehicle;

using AutoMapper;
using Car.Auction.Management.System.Contracts.Web.Vehicle.Get;
using Car.Auction.Management.System.Models.Aggregates.Vehicle;
using Car.Auction.Management.System.Models.Core;
using Car.Auction.Management.System.Models.Exceptions;
using MediatR;

public class GetVehicleByIdQueryHandler
    : IRequestHandler<GetVehicleByIdRequest, GetVehicleResponse>
{
    private readonly IRepository<Vehicle> _repository;
    private readonly IMapper _mapper;

    public GetVehicleByIdQueryHandler(
        IRepository<Vehicle> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GetVehicleResponse> Handle(GetVehicleByIdRequest request, CancellationToken cancellationToken)
    {
        var vehicle = await _repository.Get(request.VehicleId, cancellationToken);

        if (vehicle is null)
        {
            throw new NotFoundException(nameof(Vehicle), request.VehicleId);
        }

        return _mapper.Map<GetVehicleResponse>(vehicle);
    }
}