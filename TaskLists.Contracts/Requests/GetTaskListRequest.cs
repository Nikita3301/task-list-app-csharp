namespace TaskLists.Contracts.Requests;

public class GetTaskListRequest
{
    public required Guid UserId { get; init; }
}