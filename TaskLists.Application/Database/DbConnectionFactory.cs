using MongoDB.Driver;

namespace TaskLists.Application.Database;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;
    private readonly string _databaseName;

    public DbConnectionFactory(string connectionString, string databaseName)
    {
        _connectionString = connectionString;
        _databaseName = databaseName;
    }

    public IMongoDatabase CreateConnectionAsync()
    {
        var client = new MongoClient(_connectionString);
        return client.GetDatabase(_databaseName);
    }
}