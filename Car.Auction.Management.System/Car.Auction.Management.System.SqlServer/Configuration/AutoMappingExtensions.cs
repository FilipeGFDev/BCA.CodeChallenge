namespace Car.Auction.Management.System.SqlServer.Configuration;

using Microsoft.Extensions.DependencyInjection;
using Car.Auction.Management.System.SqlServer.Mappings.Vehicle;

public static class AutoMappingExtensions
{
    public static IServiceCollection AddMappingConverters(this IServiceCollection services)
    {
        services.AddScoped<VehicleConverter>();
        return services;
    }
}