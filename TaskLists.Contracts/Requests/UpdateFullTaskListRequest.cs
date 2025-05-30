namespace TaskLists.Contracts.Requests;

public class UpdateFullTaskListRequest
{
    public required Guid UserId { get; init; }
    public required string ListName { get; init; }
  
    public List<CreateTaskRequest>? Tasks { get; init; }
}