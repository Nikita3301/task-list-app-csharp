using MongoDB.Bson.Serialization.Attributes;

namespace TaskLists.Application.Models;

public class TaskListConnection
{
    [BsonId] public required Guid ListId { get; set; }
    public required Guid OwnerId { get; set; }
    public List<Guid>? ConnectedUserIds { get; set; }
}