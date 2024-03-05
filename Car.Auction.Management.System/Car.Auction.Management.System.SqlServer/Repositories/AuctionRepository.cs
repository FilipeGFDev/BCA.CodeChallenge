namespace Car.Auction.Management.System.SqlServer.Repositories;

using Microsoft.EntityFrameworkCore;
using Car.Auction.Management.System.Contracts.Web.Paged;
using Car.Auction.Management.System.Models.Aggregates.Auction;
using Car.Auction.Management.System.Models.Core;
using Car.Auction.Management.System.SqlServer.Core;
using global::System.Linq.Expressions;

public class AuctionRepository : IRepository<Auction>
{
    private readonly AppDbContext _dbContext;
    private readonly DbSet<Auction> _dbSet;

    public AuctionRepository(AppDbContext context)
    {
        _dbContext = context;
        _dbSet = _dbContext.Set<Auction>();
    }
    
    public async Task<Auction?> Get(Guid id, CancellationToken cancellationToken)
        => await _dbSet
            .Where(x => x.Id == id)
            .Include(x => x.Vehicle)
            .Include(x => x.Bids)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<PagedQueryResult<Auction>> Get(
        PageInformation pageInformation,
        IEnumerable<Expression<Func<Auction, bool>>> filters,
        Func<IQueryable<Auction>, IOrderedQueryable<Auction>>? orderBy,
        CancellationToken cancellationToken)
    {
        var auctions = _dbSet.AsQueryable();

        auctions = filters.Aggregate(
            auctions,
            (current, filter) => current.Where(filter));

        if (orderBy is not null)
        {
            auctions = orderBy(auctions);
        }

        return new(
            await auctions.CountAsync(cancellationToken),
            await auctions
                .Skip((pageInformation.Page - 1) * pageInformation.PageSize)
                .Take(pageInformation.PageSize)
                .ToListAsync(cancellationToken));
    }

    public async Task<Auction?> Create(Auction entity, CancellationToken cancellationToken)
    {
        var entry = await _dbSet.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return entry.Entity;
    }

    public async Task Update(Auction entity, CancellationToken cancellationToken)
    {
        _dbSet.Update(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}