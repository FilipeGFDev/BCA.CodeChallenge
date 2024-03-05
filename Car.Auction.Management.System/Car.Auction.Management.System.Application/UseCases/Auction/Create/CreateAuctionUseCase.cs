namespace Car.Auction.Management.System.Application.UseCases.Auction.Create;

using Car.Auction.Management.System.Application.Core;
using Car.Auction.Management.System.Models.Aggregates.Auction;
using Car.Auction.Management.System.Models.Aggregates.Vehicle;
using Car.Auction.Management.System.Models.Core;
using FluentValidation;

public class CreateAuctionUseCase : IUseCase<CreateAuctionInput, AuctionCreatedUseCaseEvent>
{
    private readonly IRepository<Auction> _auctionRepository;
    private readonly IRepository<Vehicle> _vehicleRepository;

    public CreateAuctionUseCase(
        IRepository<Auction> auctionRepository,
        IRepository<Vehicle> vehicleRepository)
    {
        _auctionRepository = auctionRepository;
        _vehicleRepository = vehicleRepository;
    }

    public async Task<UseCaseResult<AuctionCreatedUseCaseEvent>> Handle(
        CreateAuctionInput request,
        CancellationToken cancellationToken)
    {
        var auction = await _auctionRepository.Create(new(request.Proposal), cancellationToken);

        await UpdateVehicleAvailabilityAsync(auction, cancellationToken);

        return this.Result(new(auction!.Id));
    }

    private async Task UpdateVehicleAvailabilityAsync(Auction? auction, CancellationToken cancellationToken)
    {
        auction!.Vehicle.ChangeToUnavailable();

        await _vehicleRepository.Update(auction.Vehicle, cancellationToken);
    }
}