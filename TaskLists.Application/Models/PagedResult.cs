namespace TaskLists.Application.Models;

public class PagedResult<T>
{
    public required List<T> Items { get; set; }
    public required int Page { get; set; }
    public required int PageSize { get; set; }
    public required long TotalItemsCount { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalItemsCount / (double)PageSize);
    public bool HasNextPage => TotalItemsCount > (Page * PageSize);

}