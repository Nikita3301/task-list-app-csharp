namespace TaskLists.Contracts.Requests;

public class DeleteTaskListRequest
{
    public required Guid UserId { get; init; }

}