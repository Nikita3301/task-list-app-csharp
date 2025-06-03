namespace TaskLists.Contracts.Requests;

public class CreateUserRequest
{
    public required string FullName { get; init; }
}