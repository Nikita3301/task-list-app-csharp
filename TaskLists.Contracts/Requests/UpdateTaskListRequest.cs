namespace TaskLists.Contracts.Requests;

public class UpdateTaskListRequest
{
    public required Guid UserId { get; init; }
    public required string ListName { get; init; }

}