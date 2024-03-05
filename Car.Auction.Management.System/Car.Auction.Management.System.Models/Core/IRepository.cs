namespace Car.Auction.Management.System.Models.Core;

using Car.Auction.Management.System.Contracts.Web.Paged;
using global::System.Linq.Expressions;

public interface IRepository<T> where T : class
{
    Task<T?> Get(Guid id, CancellationToken cancellationToken);

    Task<PagedQueryResult<T>> Get(
        PageInformation pageInformation,
        IEnumerable<Expression<Func<T, bool>>> filters,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
        CancellationToken cancellationToken);

    Task<T?> Create(T entity, CancellationToken cancellationToken);

    Task Update(T entity, CancellationToken cancellationToken);
}