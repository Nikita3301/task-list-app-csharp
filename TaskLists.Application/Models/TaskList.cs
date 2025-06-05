using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace TaskLists.Application.Models;

public class TaskList
{
    [BsonId] public Guid Id { get; init; }
    public required string Name { get; set; }
    public required Guid OwnerId { get; set; }
    public List<TaskItem>? Tasks { get; set; }
    public List<User> ConnectedUsers { get; set; } = [];
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public override string ToString()
    {
        return $"TaskList: {Name}, Owner: {OwnerId}, CreatedAt: {CreatedAt}, Tasks: {Tasks?.Count ?? 0}, ConnectedUsers: {ConnectedUsers?.Count ?? 0}";
    }
}