using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TaskLists.Application.Database;
using TaskLists.Application.Repositories;
using TaskLists.Application.Services;

namespace TaskLists.Application;

public static class ApplicationServiceCollectionExtensions
{
    
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<ITaskListRepository,TaskListRepository>();
        services.AddSingleton<ITaskListService, TaskListService>();
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<IUserService, UserService>();
        services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);
        
        return services;
    }
    
    
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString, string databaseName)
    {
        services.AddSingleton<IDbConnectionFactory>(_ => 
            new DbConnectionFactory(connectionString, databaseName));
        return services;
    }
}