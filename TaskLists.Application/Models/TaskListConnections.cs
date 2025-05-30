using MongoDB.Bson.Serialization.Attributes;

namespace TaskLists.Application.Models;

public class TaskListConnections
{
    [BsonId] public required Guid ListId { get; set; }
    public List<Guid>? ConnectedUserIds { get; set; }
}