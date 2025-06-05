namespace TaskLists.Contracts.Responses;

public class PagedResponse <TResponse>
{
    public required IEnumerable<TResponse> Items { get; init; } = new List<TResponse>();
    public required int Page { get; init; }
    public required int PageSize { get; init; }
    public required long TotalItemsCount { get; init; }
    public int TotalPages { get; init; }
    public bool HasNextPage { get; init; }
}