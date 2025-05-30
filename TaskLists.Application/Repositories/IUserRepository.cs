namespace TaskLists.Application.Repositories;

public interface IUserRepository
{
    Task<bool> CreateAsync(Guid id, string name);
    
    Task<bool> ExistsByIdAsync(Guid id);
    
}