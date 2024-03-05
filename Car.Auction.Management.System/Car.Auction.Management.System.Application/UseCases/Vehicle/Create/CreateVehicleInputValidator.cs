namespace Car.Auction.Management.System.Application.UseCases.Vehicle.Create;

using Car.Auction.Management.System.Contracts.Web.Vehicle;
using Car.Auction.Management.System.Models.ErrorCodes;
using FluentValidation;

public class CreateVehicleInputValidator : AbstractValidator<CreateVehicleInput>
{
    public CreateVehicleInputValidator()
    {
        RuleFor(x => x.Proposal.VehicleType.Value)
            .Must(x => x is not VehicleType.None)
            .WithErrorCode(ErrorCodes.Vehicle.InvalidVehicleType.Code)
            .WithMessage(ErrorCodes.Vehicle.InvalidVehicleType.ErrorMessage)
            .DependentRules(
                () =>
                {
                    RuleFor(x => x.Proposal.VehicleType.Value)
                        .IsInEnum()
                        .WithErrorCode(ErrorCodes.Vehicle.MandatoryVehicleType.Code)
                        .WithMessage(ErrorCodes.Vehicle.MandatoryVehicleType.ErrorMessage);
                });

        RuleFor(x => x.Proposal.Manufacturer.Value)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithErrorCode(ErrorCodes.Vehicle.MandatoryVehicleManufacturer.Code)
            .WithMessage(ErrorCodes.Vehicle.MandatoryVehicleManufacturer.ErrorMessage);

        RuleFor(x => x.Proposal.Model.Value)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithErrorCode(ErrorCodes.Vehicle.MandatoryVehicleModel.Code)
            .WithMessage(ErrorCodes.Vehicle.MandatoryVehicleModel.ErrorMessage);

        RuleFor(x => x.Proposal.Year.Value)
            .GreaterThanOrEqualTo(1885)
            .WithErrorCode(ErrorCodes.Vehicle.InvalidVehicleYear.Code)
            .WithMessage(ErrorCodes.Vehicle.InvalidVehicleYear.ErrorMessage);

        RuleFor(x => x.Proposal.StartingBid.Value)
            .GreaterThanOrEqualTo(1)
            .WithErrorCode(ErrorCodes.Vehicle.InvalidVehicleStartingBid.Code)
            .WithMessage(ErrorCodes.Vehicle.InvalidVehicleStartingBid.ErrorMessage);

        RuleFor(x => x.Proposal.DoorsNumber.Value)
            .GreaterThanOrEqualTo(3)
            .WithErrorCode(ErrorCodes.Vehicle.InvalidVehicleDoorsNumber.Code)
            .WithMessage(ErrorCodes.Vehicle.InvalidVehicleDoorsNumber.ErrorMessage)
            .When(x => x.Proposal.VehicleType.Value is VehicleType.Hatchback or VehicleType.Sedan);

        RuleFor(x => x.Proposal.SeatsNumber.Value)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodes.Vehicle.InvalidVehicleSeatsNumber.Code)
            .WithMessage(ErrorCodes.Vehicle.InvalidVehicleSeatsNumber.ErrorMessage)
            .When(x => x.Proposal.VehicleType.Value is VehicleType.Suv);

        RuleFor(x => x.Proposal.LoadCapacity.Value)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodes.Vehicle.InvalidVehicleLoadCapacity.Code)
            .WithMessage(ErrorCodes.Vehicle.InvalidVehicleLoadCapacity.ErrorMessage)
            .When(x => x.Proposal.VehicleType.Value is VehicleType.Truck);
    }
}