namespace Car.Auction.Management.System.Models.Core;

public class PagedQueryResult<T>
{
    public int TotalItems { get; }

    public IEnumerable<T> Entries { get; }

    public PagedQueryResult(
        int totalItems,
        IEnumerable<T> entries)
    {
        TotalItems = totalItems;
        Entries = entries;
    }
}