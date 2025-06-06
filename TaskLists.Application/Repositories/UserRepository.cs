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


    public async Task<bool> CreateAsync(string fullName, CancellationToken token)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<User>("Users");

        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = fullName,
        };

        await collection.InsertOneAsync(user, cancellationToken: token);

        return true;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken token)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<User>("Users");

        return await collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken: token);

    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token)
    {
        var database = _dbConnectionFactory.CreateConnectionAsync();
        var collection = database.GetCollection<User>("Users");

        var exists = await collection.Find(x => x.Id == id).AnyAsync(cancellationToken: token);
        return exists;
    }
}