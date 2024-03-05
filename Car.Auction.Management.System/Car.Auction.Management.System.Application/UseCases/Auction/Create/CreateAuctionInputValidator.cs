namespace Car.Auction.Management.System.Application.UseCases.Auction.Create;

using Car.Auction.Management.System.Contracts.Web.Paged;
using Car.Auction.Management.System.Models.Aggregates.Auction;
using Car.Auction.Management.System.Models.Aggregates.Vehicle;
using Car.Auction.Management.System.Models.Core;
using Car.Auction.Management.System.Models.ErrorCodes;
using FluentValidation;
using FluentValidation.Results;
using global::System.Linq.Expressions;

public class CreateAuctionInputValidator : AbstractValidator<CreateAuctionInput>
{
    private readonly IRepository<Auction> _auctionRepository;
    private readonly IRepository<Vehicle> _vehicleRepository;

    public CreateAuctionInputValidator(
        IRepository<Auction> auctionRepository,
        IRepository<Vehicle> vehicleRepository)
    {
        _auctionRepository = auctionRepository;
        _vehicleRepository = vehicleRepository;

        RuleFor(x => x.Proposal.VehicleId)
            .Must(x => x.IsSet)
            .WithErrorCode(ErrorCodes.Auction.MandatoryAuctionVehicleId.Code)
            .WithMessage(ErrorCodes.Auction.MandatoryAuctionVehicleId.ErrorMessage)
            .DependentRules(
                () => RuleFor(x => x.Proposal.VehicleId.Value)
                    .Must(x => x != Guid.Empty)
                    .WithErrorCode(ErrorCodes.Auction.InvalidAuctionVehicleId.Code)
                    .WithMessage(ErrorCodes.Auction.InvalidAuctionVehicleId.ErrorMessage)
                    .When(x => x.Proposal.VehicleId.IsSet)
                    .DependentRules(() =>
                        RuleFor(x => x.Proposal.VehicleId.Value)
                            .CustomAsync(ValidateVehicleAsync)));
    }

    private async Task ValidateVehicleAsync(
        Guid? vehicleId,
        ValidationContext<CreateAuctionInput> context,
        CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.Get(vehicleId!.Value, cancellationToken);

        if (vehicle is null)
        {
            context.AddFailure(
                new ValidationFailure(
                    string.Empty,
                    ErrorCodes.Auction.AuctionVehicleNotFound.ErrorMessage,
                    vehicleId.Value)
                {
                    ErrorCode = ErrorCodes.Auction.AuctionVehicleNotFound.Code,
                    CustomState = $"VehicleId: \"{vehicleId.Value}\"",
                });

            return;
        }

        if (!vehicle.IsAvailable)
        {
            context.AddFailure(
                new ValidationFailure(
                    string.Empty,
                    ErrorCodes.Auction.AuctionVehicleIsUnavailable.ErrorMessage,
                    vehicleId)
                {
                    ErrorCode = ErrorCodes.Auction.AuctionVehicleIsUnavailable.Code,
                    CustomState = $"VehicleId: \"{vehicleId}\"",
                });

            return;
        }

        var existentAuctionsWithVehicleId = await _auctionRepository.Get(
            new PageInformation(),
            new List<Expression<Func<Auction, bool>>>
            {
                x => x.VehicleId == vehicleId,
            },
            null,
            cancellationToken);

        if (existentAuctionsWithVehicleId.Entries.Any())
        {
            context.AddFailure(
                new ValidationFailure(
                    string.Empty,
                    ErrorCodes.Auction.AuctionVehicleFoundedOnExistentAuction.ErrorMessage,
                    vehicleId)
                {
                    ErrorCode = ErrorCodes.Auction.AuctionVehicleFoundedOnExistentAuction.Code,
                    CustomState = $"VehicleId: \"{vehicleId}\"",
                });
        }
    }
}