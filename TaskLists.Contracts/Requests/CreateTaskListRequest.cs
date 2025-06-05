using TaskLists.Application.Models;

namespace TaskLists.Contracts.Requests;

public class CreateTaskListRequest
{
    public required Guid UserId { get; init; }
    public required string Name { get; init; }
    public List<CreateTaskRequest>? Tasks { get; init; }
    public List<User>? ConnectedUsers { get; init; }
}