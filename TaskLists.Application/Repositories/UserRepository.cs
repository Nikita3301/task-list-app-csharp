using MongoDB.Bson;
using MongoDB.Driver;
using TaskLists.Application.Database;
using TaskLists.Application.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskLists.Application.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public UserRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }


    public async Task<bool> CreateAsync(string fullName)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<User>("Users");

        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = fullName,
        };
        try
        {
            await collection.InsertOneAsync(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

        return true;
    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<User>("Users");

        var exists = await collection.Find(x => x.Id == id).AnyAsync();
        return exists;

        // var filter = Builders<User>.Filter.Eq("Id", id);
        // var userExist = collection.Find(filter).CountDocuments();
        // return Task.FromResult(userExist > 0);
    }
}