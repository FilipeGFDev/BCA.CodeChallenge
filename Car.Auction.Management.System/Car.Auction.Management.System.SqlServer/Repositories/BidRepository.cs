namespace Car.Auction.Management.System.SqlServer.Repositories;

using Microsoft.EntityFrameworkCore;
using Car.Auction.Management.System.Contracts.Web.Paged;
using Car.Auction.Management.System.Models.Aggregates.Bid;
using Car.Auction.Management.System.Models.Core;
using Car.Auction.Management.System.SqlServer.Core;
using global::System.Linq.Expressions;

public class BidRepository : IRepository<Bid>
{
    private readonly AppDbContext _dbContext;
    private readonly DbSet<Bid> _dbSet;

    public BidRepository(AppDbContext context)
    {
        _dbContext = context;
        _dbSet = _dbContext.Set<Bid>();
    }

    public async Task<Bid?> Get(Guid id, CancellationToken cancellationToken)
        => await _dbSet
            .Where(x => x.Id == id)
            .Include(x => x.Auction)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<PagedQueryResult<Bid>> Get(
        PageInformation pageInformation,
        IEnumerable<Expression<Func<Bid, bool>>> filters,
        Func<IQueryable<Bid>, IOrderedQueryable<Bid>>? orderBy,
        CancellationToken cancellationToken)
    {
        var bids = _dbSet.AsQueryable();

        bids = filters.Aggregate(
            bids,
            (current, filter) => current.Where(filter));

        if (orderBy is not null)
        {
            bids = orderBy(bids);
        }
        
        return new(
            await bids.CountAsync(cancellationToken),
            await bids
                .Skip((pageInformation.Page - 1) * pageInformation.PageSize)
                .Take(pageInformation.PageSize)
                .ToListAsync(cancellationToken));
    }

    public async Task<Bid?> Create(Bid entity, CancellationToken cancellationToken)
    {
        var entry = await _dbSet.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return entry.Entity;
    }

    public Task Update(Bid entity, CancellationToken cancellationToken) => throw new NotImplementedException();
}