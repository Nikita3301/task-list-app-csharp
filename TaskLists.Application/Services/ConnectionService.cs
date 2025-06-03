using TaskLists.Application.Exceptions;
using TaskLists.Application.Models;
using TaskLists.Application.Repositories;

namespace TaskLists.Application.Services;

public class ConnectionService : IConnectionService
{
    private readonly IConnectionRepository _connectionRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITaskListRepository _taskListRepository;

    public ConnectionService(IConnectionRepository connectionRepository, IUserRepository userRepository,
        ITaskListRepository taskListRepository)
    {
        _connectionRepository = connectionRepository;
        _userRepository = userRepository;
        _taskListRepository = taskListRepository;
    }


    public async Task<bool> CreateAsync(Guid userId, Guid taskListId, Guid otherUserId)
    {
        var userExists = await _userRepository.ExistsByIdAsync(userId);
        var otherUserExists = await _userRepository.ExistsByIdAsync(otherUserId);
        var taskListExists = await _taskListRepository.ExistsByIdAsync(taskListId);

        if (!userExists || !otherUserExists)
        {
            throw new UserNotFoundException("User not found");
        }

        if (!taskListExists)
        {
            throw new TaskListNotFoundException("TaskList not found");
        }

        var connectionExist = await _connectionRepository.ConnectionExistAsync(taskListId);
        var ownerId = await _taskListRepository.GetOwnerIdAsync(taskListId);
        if (!connectionExist)
        {
            if (userId != ownerId)
            {
                throw new NoPermissionException();
            }
            

            return await _connectionRepository.CreateAsync(taskListId, ownerId, otherUserId);
        }

        var hasPermission = await _connectionRepository.HasUserPermissionAsync(userId, taskListId);
        if (!hasPermission)
        {
            throw new NoPermissionException();
        }

        return await _connectionRepository.UpdateAsync(taskListId, otherUserId);
    }

    public async Task<TaskListConnection?> GetAsync(Guid userId, Guid taskListId)
    {
        var userExist = await _userRepository.ExistsByIdAsync(userId);
        var taskListExists = await _taskListRepository.ExistsByIdAsync(taskListId);
        if (!userExist || !taskListExists)
        {
            return null;
        }
        
        var hasPermission = await _connectionRepository.HasUserPermissionAsync(userId, taskListId);
        if (!hasPermission)
        {
            return null;
        }

        return await _connectionRepository.GetAsync(taskListId);
    }

    public async Task<bool> DeleteAsync(Guid userId, Guid taskListId, Guid userIdToDelete)
    {
        var userExists = await _userRepository.ExistsByIdAsync(userId);
        var userIdToDeleteExists = await _userRepository.ExistsByIdAsync(userIdToDelete);
        var taskListExists = await _taskListRepository.ExistsByIdAsync(taskListId);

        if (!userExists || !userIdToDeleteExists || !taskListExists)
        {
            return false;
        }

        var connectionExist = await _connectionRepository.ConnectionExistAsync(taskListId);
        if (!connectionExist)
        {
            return false;
        }
        
        var hasConnectionWithUser = await _connectionRepository.HasConnectionWithUserAsync(userIdToDelete, taskListId);
        if (!hasConnectionWithUser)
        {
            return false;
        }

        var hasPermission = await _connectionRepository.HasUserPermissionAsync(userId, taskListId);
        if (!hasPermission)
        {
            return false;
        }

        return await _connectionRepository.DeleteAsync(taskListId, userIdToDelete);
    }
}