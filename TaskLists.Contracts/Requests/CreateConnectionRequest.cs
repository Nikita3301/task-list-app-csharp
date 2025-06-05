namespace TaskLists.Contracts.Requests;

public class CreateConnectionRequest
{
    public required Guid UserId { get; init; }
    public required Guid OtherUserId { get; init; }
}