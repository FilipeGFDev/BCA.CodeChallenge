namespace Car.Auction.Management.System.SqlServer.Core;

using Car.Auction.Management.System.Contracts.Web.Vehicle;
using Car.Auction.Management.System.Models.Aggregates.Auction;
using Car.Auction.Management.System.Models.Aggregates.Bid;
using Car.Auction.Management.System.Models.Aggregates.Vehicle;
using Microsoft.EntityFrameworkCore;
using global::System.Reflection;

public class AppDbContext : DbContext
{
    public DbSet<Auction> Auctions { get; set; }
    public DbSet<Bid> Bids { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        ConfigureAuctionsTable(modelBuilder);
        ConfigureBidsTable(modelBuilder);
        ConfigureVehiclesTable(modelBuilder);
    }

    private static void ConfigureAuctionsTable(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Auction>()
            .HasKey(x => x.Id);
        modelBuilder
            .Entity<Auction>()
            .Property(x => x.Description);
        modelBuilder
            .Entity<Auction>()
            .HasOne(x => x.Vehicle)
            .WithOne()
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder
            .Entity<Auction>()
            .HasMany(x => x.Bids)
            .WithOne(x => x.Auction)
            .HasForeignKey(r => r.AuctionId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder
            .Entity<Auction>()
            .Property(x => x.VehicleId);
        modelBuilder
            .Entity<Auction>()
            .Property(x => x.StartedAt);
        modelBuilder
            .Entity<Auction>()
            .Property(x => x.ClosedAt);
        modelBuilder
            .Entity<Auction>()
            .Property(x => x.CreatedAt)
            .IsRequired();
        modelBuilder
            .Entity<Auction>()
            .Property(x => x.IsActive)
            .IsRequired();
    }

    private static void ConfigureBidsTable(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Bid>()
            .HasKey(x => x.Id);
        modelBuilder
            .Entity<Bid>()
            .HasOne(x => x.Auction)
            .WithMany(x => x.Bids)
            .HasForeignKey(x => x.AuctionId)
            .IsRequired();
        modelBuilder
            .Entity<Bid>()
            .Property(x => x.UserId)
            .IsRequired();
        modelBuilder
            .Entity<Bid>()
            .Property(x => x.Amount)
            .IsRequired();
        modelBuilder
            .Entity<Bid>()
            .Property(x => x.CreatedAt)
            .IsRequired();
    }

    private static void ConfigureVehiclesTable(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Vehicle>()
            .HasKey(x => x.Id);
        modelBuilder
            .Entity<Vehicle>()
            .Property(x => x.Manufacturer)
            .IsRequired();
        modelBuilder
            .Entity<Vehicle>()
            .Property(x => x.Model)
            .IsRequired();
        modelBuilder
            .Entity<Vehicle>()
            .Property(x => x.Year)
            .IsRequired();
        modelBuilder
            .Entity<Vehicle>()
            .Property(x => x.StartingBid)
            .IsRequired();
        modelBuilder
            .Entity<Vehicle>()
            .Property(x => x.IsAvailable)
            .IsRequired();
        modelBuilder
            .Entity<Vehicle>()
            .HasDiscriminator<string>(nameof(VehicleType));
        modelBuilder
            .Entity<Hatchback>()
            .Property(x => x.DoorsNumber)
            .HasColumnName(nameof(Hatchback.DoorsNumber))
            .IsRequired();
        modelBuilder
            .Entity<Hatchback>()
            .HasDiscriminator()
            .HasValue(nameof(VehicleType.Hatchback));
        modelBuilder
            .Entity<Sedan>()
            .Property(x => x.DoorsNumber)
            .HasColumnName(nameof(Sedan.DoorsNumber))
            .IsRequired();
        modelBuilder
            .Entity<Sedan>()
            .HasDiscriminator()
            .HasValue(nameof(VehicleType.Sedan));
        modelBuilder
            .Entity<Suv>()
            .Property(x => x.SeatsNumber)
            .IsRequired();
        modelBuilder
            .Entity<Suv>()
            .HasDiscriminator()
            .HasValue(nameof(VehicleType.Suv));
        modelBuilder
            .Entity<Truck>()
            .Property(x => x.LoadCapacity)
            .IsRequired();
        modelBuilder
            .Entity<Truck>()
            .HasDiscriminator()
            .HasValue(nameof(VehicleType.Truck));
        modelBuilder
            .Entity<Vehicle>()
            .Property(x => x.CreatedAt)
            .IsRequired();
    }
}