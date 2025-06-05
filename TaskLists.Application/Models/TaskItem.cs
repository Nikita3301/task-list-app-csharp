using MongoDB.Bson.Serialization.Attributes;

namespace TaskLists.Application.Models;

public class TaskItem
{
    public required string Description { get; set; }
    public required bool Completed { get; set; }
}