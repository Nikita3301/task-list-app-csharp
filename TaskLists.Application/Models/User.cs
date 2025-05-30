using MongoDB.Bson.Serialization.Attributes;

namespace TaskLists.Application.Models;

public class User
{
    [BsonId] public required Guid Id { get; init; }
    public string? FullName { get; set; }
}