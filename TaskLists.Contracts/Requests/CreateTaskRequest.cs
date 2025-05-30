namespace TaskLists.Contracts.Requests;

public class CreateTaskRequest
{
    public required string Description { get; init; }
    public required bool Completed { get; init; }
}