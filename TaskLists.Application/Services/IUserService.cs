namespace TaskLists.Application.Services;

public interface IUserService
{
    Task<bool> CreateAsync(string name);
    Task<bool> ExistsByIdAsync(Guid id);
}