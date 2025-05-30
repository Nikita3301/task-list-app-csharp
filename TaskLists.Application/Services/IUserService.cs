namespace TaskLists.Application.Services;

public interface IUserService
{
    Task<bool> CreateAsync(Guid id, string name);
    Task<bool> ExistsByIdAsync(Guid id);
}