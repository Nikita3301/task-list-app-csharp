using TaskLists.Application.Models;

namespace TaskLists.Application.Repositories;

public interface IUserRepository
{
    Task<bool> CreateAsync(string name);
    Task<User?> GetByIdAsync(Guid id);
    Task<bool> ExistsByIdAsync(Guid id);
}