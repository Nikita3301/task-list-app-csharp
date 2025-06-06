using TaskLists.Application.Models;

namespace TaskLists.Application.Repositories;

public interface IUserRepository
{
    Task<bool> CreateAsync(string name, CancellationToken token);
    Task<User?> GetByIdAsync(Guid id, CancellationToken token);
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken token);
}