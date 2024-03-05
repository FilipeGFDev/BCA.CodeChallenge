namespace Car.Auction.Management.System.Contracts.Web.Paged;

public interface IPagedRequest
{
    int? Page { get; }

    int? PageSize { get; }
}