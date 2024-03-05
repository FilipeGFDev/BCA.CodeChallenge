namespace Car.Auction.Management.System.SqlServer.Configuration;

using Microsoft.Extensions.DependencyInjection;
using Car.Auction.Management.System.Models.Aggregates.Auction;
using Car.Auction.Management.System.Models.Aggregates.Bid;
using Car.Auction.Management.System.Models.Aggregates.Vehicle;
using Car.Auction.Management.System.Models.Core;
using Car.Auction.Management.System.SqlServer.Repositories;

public static class SqlServerServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepository<Auction>, AuctionRepository>();
        services.AddScoped<IRepository<Bid>, BidRepository>();
        services.AddScoped<IRepository<Vehicle>, VehicleRepository>();
        return services;
    }
}