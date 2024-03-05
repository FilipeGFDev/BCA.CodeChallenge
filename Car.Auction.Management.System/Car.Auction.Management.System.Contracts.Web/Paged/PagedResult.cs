namespace Car.Auction.Management.System.Contracts.Web.Paged;

public class PagedResult<T>
{
    private readonly PageInformation Page;

    public int TotalPages { get; }

    public int TotalItems { get; }

    public IEnumerable<T> Entries { get; }

    public PagedResult(
        PageInformation page,
        int totalItems,
        IEnumerable<T> entries)
    {
        Page = page;
        TotalItems = totalItems;
        TotalPages = (TotalItems + page.PageSize - 1) / page.PageSize;
        Entries = entries;
    }
}