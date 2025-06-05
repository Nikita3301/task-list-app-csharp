namespace TaskLists.Contracts.Requests;

public class GetAllConnectionsRequest
{
    public required Guid UserId { get; init; }
    public required Guid ListId { get; init; }
}