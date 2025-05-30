using TaskLists.Application.Models;
using TaskLists.Application.Repositories;

namespace TaskLists.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> CreateAsync(Guid id, string name)
    {
        return await _userRepository.CreateAsync(id, name);
    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        return await _userRepository.ExistsByIdAsync(id);
    }
}