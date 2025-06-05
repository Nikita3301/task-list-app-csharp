using TaskLists.Application.Models;

namespace TaskLists.Contracts.Responses;

public class TaskListResponse
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public Guid OwnerId { get; init; }
    public List<TaskItemResponse>? Tasks { get; init; }
    public List<User>? ConnectedUsers { get; init; }
    public DateTime CreatedAt { get; init; }
}