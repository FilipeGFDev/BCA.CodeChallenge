namespace Car.Auction.Management.System.Application.Core;

using Car.Auction.Management.System.Application.UseCases.Vehicle.Create;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

public static class FluentValidationConfiguration
{
    public static IServiceCollection AddFluentValidators(this IServiceCollection serviceCollection)
        => serviceCollection.AddValidatorsFromAssemblies(new[] { typeof(CreateVehicleInputValidator).Assembly });
}