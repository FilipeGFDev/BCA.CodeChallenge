namespace Car.Auction.Management.System.Application.UseCases.Vehicle.Create;

using Car.Auction.Management.System.Application.Core;
using Car.Auction.Management.System.Application.Factories;
using Car.Auction.Management.System.Models.Aggregates.Vehicle;
using Car.Auction.Management.System.Models.Core;

public class CreateVehicleUseCase : IUseCase<CreateVehicleInput, VehicleCreatedUseCaseEvent>
{
    private readonly IRepository<Vehicle> _repository;

    public CreateVehicleUseCase(IRepository<Vehicle> repository)
    {
        _repository = repository;
    }

    public async Task<UseCaseResult<VehicleCreatedUseCaseEvent>> Handle(
        CreateVehicleInput request,
        CancellationToken cancellationToken)
    {
        var vehicle = VehicleFactory.CreateVehicle(request.Proposal);

        await _repository.Create(vehicle, cancellationToken);

        return this.Result(new(vehicle.Id));
    }
}