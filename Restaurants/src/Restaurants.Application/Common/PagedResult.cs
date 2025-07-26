namespace Restaurants.Application.Common;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = [];
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
    public int ItemsFrom { get; set; }
    public int ItemsTo { get; set; }

    public PagedResult(IEnumerable<T> items, int totalCount, int pageSize, int pageNumber)
    {
        Items = items;
        TotalItems = totalCount;
        TotalPages = (int)Math.Ceiling((double)TotalItems / pageSize);
        ItemsFrom = pageSize * (pageNumber - 1) + 1;
        ItemsTo = ItemsFrom + pageSize - 1;
    }
}
