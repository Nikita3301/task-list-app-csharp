namespace TaskLists.Application.Services;

public interface IUserService
{
    Task<bool> CreateAsync(string name);
}