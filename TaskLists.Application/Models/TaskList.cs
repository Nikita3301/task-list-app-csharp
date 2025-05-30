using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace TaskLists.Application.Models;

public class TaskList
{
    [BsonId] public Guid ListId { get; init; } = Guid.NewGuid();
    public required string ListName { get; set; }
    public required Guid OwnerId { get; set; }
    public List<TaskItem>? Tasks { get; set; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}