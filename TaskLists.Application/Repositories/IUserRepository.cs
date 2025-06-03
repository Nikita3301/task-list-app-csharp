namespace TaskLists.Application.Repositories;

public interface IUserRepository
{
    Task<bool> CreateAsync(string name);
    
    Task<bool> ExistsByIdAsync(Guid id);
    
}