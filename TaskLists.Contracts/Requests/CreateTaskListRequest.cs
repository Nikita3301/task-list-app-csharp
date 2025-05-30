namespace TaskLists.Contracts.Requests;

public class CreateTaskListRequest
{
    public required Guid UserId { get; init; }
    public required string ListName { get; init; }
    public List<CreateTaskRequest>? Tasks { get; init; }
}