using MongoDB.Driver;
using MongoDB.Driver.Linq;
using TaskLists.Application.Database;
using TaskLists.Application.Models;

namespace TaskLists.Application.Repositories;

public class TaskItemRepository : ITaskItemRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public TaskItemRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> CreateAsync(TaskItem task)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskItem>("Tasks");
        
        try
        {
            await collection.InsertOneAsync(task);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }


        return true;
        
    }

    public async Task<bool> CreateMultipleAsync(List<TaskItem> tasks, Guid listId)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskItem>("Tasks");
        
        foreach (var taskItem in tasks)
        {
            taskItem.ListId = listId;
        }
        
        try
        {
            await collection.InsertManyAsync(tasks);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }


        return true;
    }
    
    
    public Task<List<TaskItem>> GetTasksByListIdAsync(Guid listId)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskItem>("Tasks");

        var tasks = collection.AsQueryable()
            .Where(l => l.ListId == listId)
            .Select(t => new TaskItem
            {
                TaskId = t.TaskId,
                ListId = t.ListId,
                Completed = t.Completed,
                Description = t.Description,
            }).ToListAsync();
        return tasks;
    }

    public async Task<bool> ExistsTasksByListIdAsync(Guid listId)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskItem>("Tasks");

        var tasks = await collection.Find(l => l.ListId == listId).CountDocumentsAsync();
        return tasks > 0;
    }

    public async Task<bool> UpdateAllTasksByListIdAsync(List<TaskItem> tasks, Guid listId)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskItem>("Tasks");

        try
        {
            await collection.DeleteManyAsync(l => l.ListId == listId);
            await collection.InsertManyAsync(tasks);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        
        return true;
    }
}