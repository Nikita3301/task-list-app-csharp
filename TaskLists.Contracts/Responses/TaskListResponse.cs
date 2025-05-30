using TaskLists.Application.Models;

namespace TaskLists.Contracts.Responses;

public class TaskListResponse
{
    public required Guid ListId { get; init; }
    public required string ListName { get; init; }
    public required Guid OwnerId { get; init; }
    public List<TaskItemResponse>? Tasks { get; init; }
    public required DateTime CreatedAt { get; init; }
}