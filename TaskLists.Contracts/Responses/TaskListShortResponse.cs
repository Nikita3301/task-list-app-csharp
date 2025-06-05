namespace TaskLists.Contracts.Responses;

public class TaskListShortResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required DateTime CreatedAt { get; init; }
}