namespace Car.Auction.Management.System.Contracts.Web.Paged;

public class PageInformation
{
    public const int DefaultPageNumber = 1;

    public const int DefaultPageSize = 60;

    private const int MaxPageSize = 180;

    public PageInformation(IPagedRequest request)
    {
        var page = request.Page.GetValueOrDefault(DefaultPageNumber);
        var pageSize = request.PageSize.GetValueOrDefault(DefaultPageSize);

        if (page <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request.Page), page, null);
        }

        if (pageSize is <= 0 or > MaxPageSize)
        {
            throw new ArgumentOutOfRangeException(nameof(request.PageSize), pageSize, null);
        }

        Page = page;
        PageSize = pageSize;
    }

    public PageInformation(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }

    public PageInformation()
    {
        Page = DefaultPageNumber;
        PageSize = DefaultPageSize;
    }

    public int Page { get; }

    public int PageSize { get; }
}