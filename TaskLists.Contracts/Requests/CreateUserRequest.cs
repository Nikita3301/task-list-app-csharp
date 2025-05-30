namespace TaskLists.Contracts.Requests;

public class CreateUserRequest
{
    public required Guid UserId { get; init; }
    public required string FullName { get; init; }
}