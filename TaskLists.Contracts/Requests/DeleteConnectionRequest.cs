namespace TaskLists.Contracts.Requests;

public class DeleteConnectionRequest
{
    public required Guid UserId { get; init; }
    public required Guid UserIdToDelete { get; init; }
}