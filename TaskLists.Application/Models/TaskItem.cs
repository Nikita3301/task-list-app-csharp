using MongoDB.Bson.Serialization.Attributes;

namespace TaskLists.Application.Models;

public class TaskItem
{
    [BsonId] public Guid TaskId { get; init; } = Guid.NewGuid();
    public required Guid ListId { get; set; }
    public required string Description { get; set; }
    public required bool Completed { get; set; }
}