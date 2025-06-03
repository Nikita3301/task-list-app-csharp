using MongoDB.Driver;
using TaskLists.Application.Database;
using TaskLists.Application.Exceptions;
using TaskLists.Application.Models;

namespace TaskLists.Application.Repositories;

public class ConnectionRepository : IConnectionRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public ConnectionRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }


    public async Task<bool> CreateAsync(Guid taskListId, Guid ownerId, Guid otherUserId)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var taskListConnectionCollection = database.GetCollection<TaskListConnection>("TaskListConnection");

        var taskListConnection = new TaskListConnection
        {
            ListId = taskListId,
            OwnerId = ownerId,
            ConnectedUserIds = [otherUserId]
        };

        try
        {
            await taskListConnectionCollection.InsertOneAsync(taskListConnection);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    
    
    public async Task<bool> UpdateAsync(Guid taskListId, Guid otherUserId)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskListConnection>("TaskListConnection");

        try
        {
            var filter = Builders<TaskListConnection>.Filter.Eq(x => x.ListId, taskListId);
            var update = Builders<TaskListConnection>.Update.Push("ConnectedUserIds", otherUserId);
            await collection.FindOneAndUpdateAsync(filter, update);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }


    public async Task<TaskListConnection?> GetAsync(Guid taskListId)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskListConnection>("TaskListConnection");
        
        
        var connection = await collection.Find(x => x.ListId == taskListId).FirstOrDefaultAsync();
        if (connection is null)
        {
            return null;
        }

        return connection;
    }

    public async Task<bool> DeleteAsync(Guid taskListId, Guid userIdToDelete)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskListConnection>("TaskListConnection");
        
        try
        {
            var filter = Builders<TaskListConnection>.Filter.Eq(x => x.ListId, taskListId);
            var update = Builders<TaskListConnection>.Update.Pull("ConnectedUserIds", userIdToDelete);
            await collection.FindOneAndUpdateAsync(filter, update);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<bool> HasUserPermissionAsync(Guid userId, Guid taskListId)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskListConnection>("TaskListConnection");

        var hasPermission = await collection.Find(Builders<TaskListConnection>.Filter.Eq("Id", taskListId)
                                                  & Builders<TaskListConnection>.Filter.Eq("OwnerId", userId) |
                                                  Builders<TaskListConnection>.Filter.AnyEq("ConnectedUserIds", userId))
            .AnyAsync();

        return hasPermission;
    }
    
    
    public async Task<bool> HasConnectionWithUserAsync(Guid userId, Guid taskListId)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskListConnection>("TaskListConnection");

        return await collection.Find(Builders<TaskListConnection>.Filter.Eq("Id", taskListId)
                                                  & Builders<TaskListConnection>.Filter.AnyEq("ConnectedUserIds", userId))
            .AnyAsync();

        
    }

    public async Task<bool> ConnectionExistAsync(Guid taskListId)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<TaskListConnection>("TaskListConnection");

        return await collection.Find(l => l.ListId == taskListId).AnyAsync();
    }
}