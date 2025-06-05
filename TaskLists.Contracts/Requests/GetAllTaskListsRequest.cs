namespace TaskLists.Contracts.Requests;

public class GetAllTaskListsRequest : PagedRequest
{
    public required Guid UserId { get; init; }
    
}