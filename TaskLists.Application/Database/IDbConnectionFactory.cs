using MongoDB.Driver;

namespace TaskLists.Application.Database;

public interface IDbConnectionFactory
{
    IMongoDatabase CreateConnectionAsync(CancellationToken token = default);
}