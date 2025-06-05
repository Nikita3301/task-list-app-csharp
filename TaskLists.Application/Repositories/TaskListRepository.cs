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
        await taskListCollection.InsertOneAsync(taskList);
        return taskList;
    }


    public async Task<TaskList?> UpdateAsync(TaskList taskList)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskList>("TaskList");

        var filter = Builders<TaskList>.Filter.Eq(f => f.Id, taskList.Id);

        var update = Builders<TaskList>.Update.Set(f => f.Name, taskList.Name);

        await collection.UpdateOneAsync(filter, update);
        var updatedTaskList = await collection.Find(x => x.Id == taskList.Id).FirstOrDefaultAsync();

        return updatedTaskList;
    }
    
    public async Task<bool> DeleteByIdAsync(Guid listId)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskList>("TaskList");
        
        var result = await collection.DeleteOneAsync(Builders<TaskList>.Filter.Eq(f => f.Id, listId));
        return result.IsAcknowledged;
    }
    
    public async Task<TaskList?> GetByListIdAsync(Guid listId)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskList>("TaskList");
        var taskList = await collection.Find(x => x.Id == listId).FirstAsync();
        return taskList;
    }

    public async Task<PagedResult<TaskList>?> GetAllAsync(Guid userId, PageOptions options)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskList>("TaskList");

        var taskLists = collection.Find(Builders<TaskList>.Filter.ElemMatch(c => c.ConnectedUsers, u => u.Id == userId)).SortByDescending(c => c.CreatedAt);
        var totalLists = await taskLists.CountDocumentsAsync();
        var items = await taskLists.Limit(options.PageSize).Skip((options.Page - 1) * options.PageSize).ToListAsync();
        
        return new PagedResult<TaskList>()
        {
            Items = items,
            Page = options.Page,
            PageSize = options.PageSize,
            TotalItemsCount = totalLists
        };
    }

    public async Task<bool> CreateConnectionAsync(Guid taskListId, User otherUser)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskList>("TaskList");
        
        var taskList = await collection.Find(x => x.Id == taskListId).FirstAsync();
        
        taskList.ConnectedUsers.Add(otherUser);
        
        var filter = Builders<TaskList>.Filter.Eq(f => f.Id, taskListId);
        var update = Builders<TaskList>.Update.Set(c => c.ConnectedUsers, taskList.ConnectedUsers);
        var result = await collection.UpdateOneAsync(filter, update);

        return result.IsAcknowledged;
    }

    public async Task<List<User>> GetAllConnectionsAsync(Guid listId)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskList>("TaskList");
        
        var taskList = await collection.Find(x => x.Id == listId).FirstAsync();
        
        return taskList.ConnectedUsers;
    }

    public async Task<bool> DeleteConnectionsAsync(Guid listId, Guid userIdToDelete)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskList>("TaskList");
        
        var taskList = await collection.Find(x => x.Id == listId).FirstAsync();
        
        taskList.ConnectedUsers.Remove(taskList.ConnectedUsers.First(u => u.Id == userIdToDelete));
        
        var filter = Builders<TaskList>.Filter.Eq(f => f.Id, listId);
        var update = Builders<TaskList>.Update.Set(c => c.ConnectedUsers, taskList.ConnectedUsers);
        var result = await collection.UpdateOneAsync(filter, update);

        return result.IsAcknowledged;
    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskList>("TaskList");

        var exist = await collection.Find(i => i.Id == id).AnyAsync();
        return exist;
    }
}