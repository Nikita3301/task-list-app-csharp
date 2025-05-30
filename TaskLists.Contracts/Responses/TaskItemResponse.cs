namespace TaskLists.Contracts.Responses;

public class TaskItemResponse
{
    public required string Description { get; init; }
    public required bool Completed { get; init; }
}