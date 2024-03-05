namespace Car.Auction.Management.System.SqlServer.Repositories;

using Microsoft.EntityFrameworkCore;
using Car.Auction.Management.System.Contracts.Web.Paged;
using Car.Auction.Management.System.Models.Aggregates.Vehicle;
using Car.Auction.Management.System.Models.Core;
using Car.Auction.Management.System.SqlServer.Core;
using global::System.Linq.Expressions;

public class VehicleRepository : IRepository<Vehicle>
{
    private readonly AppDbContext _dbContext;
    private readonly DbSet<Vehicle> _dbSet;

    public VehicleRepository(AppDbContext context)
    {
        _dbContext = context;
        _dbSet = _dbContext.Set<Vehicle>();
    }

    public async Task<Vehicle?> Get(Guid id, CancellationToken cancellationToken)
        => await _dbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<PagedQueryResult<Vehicle>> Get(
        PageInformation pageInformation,
        IEnumerable<Expression<Func<Vehicle, bool>>> filters,
        Func<IQueryable<Vehicle>, IOrderedQueryable<Vehicle>>? orderBy,
        CancellationToken cancellationToken)
    {
        var vehicles = _dbSet.AsQueryable();

        vehicles = filters.Aggregate(
            vehicles,
            (current, filter) => current.Where(filter));

        if (orderBy is not null)
        {
            vehicles = orderBy(vehicles);
        }

        return new(
            await vehicles.CountAsync(cancellationToken),
            await vehicles
                .Skip((pageInformation.Page - 1) * pageInformation.PageSize)
                .Take(pageInformation.PageSize)
                .ToListAsync(cancellationToken));
    }

    public async Task<Vehicle?> Create(Vehicle entity, CancellationToken cancellationToken)
    {
        var entry = await _dbSet.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return entry.Entity;
    }

    public async Task Update(Vehicle entity, CancellationToken cancellationToken)
    {
        _dbSet.Update(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}