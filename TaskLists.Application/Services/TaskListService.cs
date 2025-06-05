using FluentValidation;
using TaskLists.Application.Exceptions;
using TaskLists.Application.Models;
using TaskLists.Application.Repositories;
using Task = System.Threading.Tasks.Task;

namespace TaskLists.Application.Services;

public class TaskListService : ITaskListService
{
    private readonly ITaskListRepository _taskListRepository;
    private readonly IUserRepository _userRepository;
    private readonly IValidator<TaskList> _taskListValidator;
    private readonly IValidator<PageOptions> _pageOptionsValidator;

    public TaskListService(ITaskListRepository taskListRepository, IUserRepository userRepository,
        IValidator<TaskList> taskListValidator, IValidator<PageOptions> pageOptionsValidator)
    {
        _taskListRepository = taskListRepository;
        _userRepository = userRepository;
        _taskListValidator = taskListValidator;
        _pageOptionsValidator = pageOptionsValidator;
    }


    public async Task<TaskList?> CreateAsync(Guid userId, TaskList taskList)
    {
        var owner = await _userRepository.GetByIdAsync(userId);
        if (owner is null)
        {
            throw new UserNotFoundException();
        }

        await _taskListValidator.ValidateAndThrowAsync(taskList);


        foreach (var user in taskList.ConnectedUsers)
        {
            await EnsureConnectedUserExistsAsync(user.Id);
        }

        if (taskList.ConnectedUsers.All(x => x.Id != userId))
        {
            taskList.ConnectedUsers.Add(owner);
        }


        var createdTaskList = await _taskListRepository.CreateAsync(taskList);

        if (createdTaskList is null)
        {
            throw new TaskListCreationException();
        }

        return createdTaskList;
    }

    public async Task<TaskList?> UpdateAsync(Guid userId, TaskList taskList)
    {
        await EnsureUserExistsAsync(userId);
        await EnsureTaskListExistsAsync(taskList.Id);
        await _taskListValidator.ValidateAndThrowAsync(taskList);
        await HasPermission(userId, taskList.Id);


        return await _taskListRepository.UpdateAsync(taskList);
    }

    public async Task<bool> DeleteByIdAsync(Guid userId, Guid listId)
    {
        await EnsureUserExistsAsync(userId);
        await EnsureTaskListExistsAsync(listId);
        await HasOwnerPermission(userId, listId);

        return await _taskListRepository.DeleteByIdAsync(listId);
    }

    public async Task<TaskList?> GetByListIdAsync(Guid userId, Guid listId)
    {
        await EnsureUserExistsAsync(userId);
        await EnsureTaskListExistsAsync(listId);
        await HasPermission(userId, listId);

        var taskList = await _taskListRepository.GetByListIdAsync(listId);

        return taskList;
    }


    public async Task<PagedResult<TaskList>?> GetAllAsync(Guid userId, PageOptions options)
    {
        await _pageOptionsValidator.ValidateAndThrowAsync(options);
        await EnsureUserExistsAsync(userId);

        return await _taskListRepository.GetAllAsync(userId, options);
    }

    public async Task<bool> CreateConnectionAsync(Guid listId, Guid ownerId, Guid otherUserId)
    {
        await EnsureUserExistsAsync(ownerId);
        await EnsureTaskListExistsAsync(listId);
        await HasPermission(ownerId, listId);
        var otherUser = await _userRepository.GetByIdAsync(otherUserId);
        if (otherUser is null)
        {
            throw new UserNotFoundException();
        }
        var connections = await _taskListRepository.GetAllConnectionsAsync(listId);
        if (connections.Any(u => u.Id == otherUserId))
        {
            throw new ConnectionAlreadyExistsException();
        }

        return await _taskListRepository.CreateConnectionAsync(listId, otherUser);
    }

    public async Task<List<User>> GetAllConnectionsAsync(Guid userId, Guid listId)
    {
        await EnsureUserExistsAsync(userId);
        await EnsureTaskListExistsAsync(listId);
        await HasPermission(userId, listId);

        return await _taskListRepository.GetAllConnectionsAsync(listId);
    }

    public async Task<bool> DeleteConnectionsAsync(Guid userId, Guid listId, Guid userIdToDelete)
    {
        await EnsureTaskListExistsAsync(listId);
        var taskList = await _taskListRepository.GetByListIdAsync(listId);
        await EnsureUserExistsAsync(userIdToDelete);
        if (taskList!.OwnerId == userIdToDelete)
        {
            throw new OwnerConnectionDeleteException();
        }
        await EnsureUserExistsAsync(userId);
        await HasPermission(userId, listId);

        if (taskList.ConnectedUsers.All(x => x.Id != userIdToDelete))
        {
            throw new ConnectionNotFoundException();
        }
        
        return await _taskListRepository.DeleteConnectionsAsync(listId, userIdToDelete);
    }

    private async Task HasPermission(Guid userId, Guid listId)
    {
        var taskList = await _taskListRepository.GetByListIdAsync(listId);
        if (taskList!.ConnectedUsers == null || taskList.ConnectedUsers.All(u => u.Id != userId))
        {
            throw new NoPermissionException();
        }
    }

    private async Task HasOwnerPermission(Guid userId, Guid listId)
    {
        var taskList = await _taskListRepository.GetByListIdAsync(listId);
        if (taskList!.OwnerId != userId)
        {
            throw new NoPermissionException();
        }
    }

    private async Task EnsureTaskListExistsAsync(Guid listId)
    {
        var taskListExists = await _taskListRepository.ExistsByIdAsync(listId);
        if (!taskListExists)
        {
            throw new TaskListNotFoundException();
        }
    }

    private async Task EnsureUserExistsAsync(Guid userId)
    {
        var userExists = await _userRepository.ExistsByIdAsync(userId);
        if (!userExists)
        {
            throw new UserNotFoundException();
        }
    }

    private async Task EnsureConnectedUserExistsAsync(Guid userId)
    {
        var userExists = await _userRepository.ExistsByIdAsync(userId);
        if (!userExists)
        {
            throw new ConnectedUserNotFoundException();
        }
    }
}