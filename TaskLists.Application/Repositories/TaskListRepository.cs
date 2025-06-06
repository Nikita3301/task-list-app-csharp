using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using TaskLists.Application.Database;
using TaskLists.Application.Models;

namespace TaskLists.Application.Repositories;

public class TaskListRepository : ITaskListRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public TaskListRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
  
    }

    public async Task<TaskList?> CreateAsync(TaskList taskList, CancellationToken token)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var taskListCollection = database.GetCollection<TaskList>("TaskList");
        await taskListCollection.InsertOneAsync(taskList, cancellationToken: token);
        return taskList;
    }


    public async Task<TaskList?> UpdateAsync(TaskList taskList, CancellationToken token)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskList>("TaskList");

        var filter = Builders<TaskList>.Filter.Eq(f => f.Id, taskList.Id);

        var update = Builders<TaskList>.Update.Set(f => f.Name, taskList.Name);

        await collection.UpdateOneAsync(filter, update, cancellationToken: token);
        var updatedTaskList = await collection.Find(x => x.Id == taskList.Id).FirstOrDefaultAsync(cancellationToken: token);

        return updatedTaskList;
    }
    
    public async Task<bool> DeleteByIdAsync(Guid listId, CancellationToken token)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskList>("TaskList");
        
        var result = await collection.DeleteOneAsync(Builders<TaskList>.Filter.Eq(f => f.Id, listId), token);
        return result.IsAcknowledged;
    }
    
    public async Task<TaskList?> GetByListIdAsync(Guid listId, CancellationToken token)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskList>("TaskList");
        var taskList = await collection.Find(x => x.Id == listId).FirstAsync(cancellationToken: token);
        return taskList;
    }

    public async Task<PagedResult<TaskList>?> GetAllAsync(Guid userId, PageOptions options, CancellationToken token)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskList>("TaskList");
        
        var taskLists = collection.Find(Builders<TaskList>.Filter.ElemMatch(c => c.ConnectedUsers, u => u.Id == userId)).SortByDescending(c => c.CreatedAt);
        var totalLists = await taskLists.CountDocumentsAsync(token);
        var items = await taskLists.Limit(options.PageSize).Skip((options.Page - 1) * options.PageSize).ToListAsync(cancellationToken: token);
        
        return new PagedResult<TaskList>()
        {
            Items = items,
            Page = options.Page,
            PageSize = options.PageSize,
            TotalItemsCount = totalLists
        };
    }

    public async Task<bool> CreateConnectionAsync(Guid taskListId, User otherUser, CancellationToken token)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskList>("TaskList");
        
        var taskList = await collection.Find(x => x.Id == taskListId).FirstAsync(cancellationToken: token);
        
        taskList.ConnectedUsers.Add(otherUser);
        
        var filter = Builders<TaskList>.Filter.Eq(f => f.Id, taskListId);
        var update = Builders<TaskList>.Update.Set(c => c.ConnectedUsers, taskList.ConnectedUsers);
        var result = await collection.UpdateOneAsync(filter, update, cancellationToken: token);

        return result.IsAcknowledged;
    }

    public async Task<List<User>> GetAllConnectionsAsync(Guid listId, CancellationToken token)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskList>("TaskList");
        
        var taskList = await collection.Find(x => x.Id == listId).FirstAsync(cancellationToken: token);
        
        return taskList.ConnectedUsers;
    }

    public async Task<bool> DeleteConnectionsAsync(Guid listId, Guid userIdToDelete, CancellationToken token)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskList>("TaskList");
        
        var taskList = await collection.Find(x => x.Id == listId).FirstAsync(cancellationToken: token);
        
        taskList.ConnectedUsers.Remove(taskList.ConnectedUsers.First(u => u.Id == userIdToDelete));
        
        var filter = Builders<TaskList>.Filter.Eq(f => f.Id, listId);
        var update = Builders<TaskList>.Update.Set(c => c.ConnectedUsers, taskList.ConnectedUsers);
        var result = await collection.UpdateOneAsync(filter, update, cancellationToken: token);

        return result.IsAcknowledged;
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskList>("TaskList");

        var exist = await collection.Find(i => i.Id == id).AnyAsync(cancellationToken: token);
        return exist;
    }
}