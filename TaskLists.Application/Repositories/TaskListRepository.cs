using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using TaskLists.Application.Database;
using TaskLists.Application.Models;

namespace TaskLists.Application.Repositories;

public class TaskListRepository : ITaskListRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IUserRepository _userRepository;

    public TaskListRepository(IDbConnectionFactory dbConnectionFactory, IUserRepository userRepository)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _userRepository = userRepository;
    }

    public async Task<TaskList?> CreateAsync(TaskList taskList)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var taskListCollection = database.GetCollection<TaskList>("TaskList");

        try
        {
            await taskListCollection.InsertOneAsync(taskList);
            return taskList;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public async Task<List<TaskList>?> GetByUserIdAsync(Guid userId)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskList>("TaskList");

        var document = await collection.Find(x => x.OwnerId == userId).ToListAsync();
        return document ?? null;
    }

    public async Task<TaskList?> GetByListIdAsync(Guid listId)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskList>("TaskList");


        var document = await collection.Find(x => x.ListId == listId).FirstOrDefaultAsync();
        if (document is null)
        {
            return null;
        }

        return new TaskList
        {
            ListId = document.ListId,
            ListName = document.ListName,
            OwnerId = document.OwnerId,
            CreatedAt = document.CreatedAt
        };
    }

    public async Task<bool> UpdateAsync(TaskList taskList)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskList>("TaskList");

        var filter = Builders<TaskList>.Filter.Eq(f => f.ListId, taskList.ListId);

        var update = Builders<TaskList>.Update.Set(f => f.ListName, taskList.ListName);

        await collection.UpdateOneAsync(filter, update);
        return true;
    }

    public Task<bool> DeleteAsync(TaskList taskList)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskList>("TaskList");

        var document = await collection.Find(i => i.ListId == id).FirstAsync();
        return document is not null;
    }

   
}