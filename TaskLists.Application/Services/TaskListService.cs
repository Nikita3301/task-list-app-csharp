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


    public async Task<TaskList?> CreateAsync(Guid userId, TaskList taskList, CancellationToken token)
    {
        var owner = await _userRepository.GetByIdAsync(userId, token);
        if (owner is null)
        {
            throw new UserNotFoundException();
        }

        await _taskListValidator.ValidateAndThrowAsync(taskList, cancellationToken: token);


        foreach (var user in taskList.ConnectedUsers)
        {
            await EnsureConnectedUserExistsAsync(user.Id, token);
        }

        if (taskList.ConnectedUsers.All(x => x.Id != userId))
        {
            taskList.ConnectedUsers.Add(owner);
        }


        var createdTaskList = await _taskListRepository.CreateAsync(taskList, token);

        if (createdTaskList is null)
        {
            throw new TaskListCreationException();
        }

        return createdTaskList;
    }

    public async Task<TaskList?> UpdateAsync(Guid userId, TaskList taskList, CancellationToken token)
    {
        await EnsureUserExistsAsync(userId, token);
        await EnsureTaskListExistsAsync(taskList.Id, token);
        await _taskListValidator.ValidateAndThrowAsync(taskList, cancellationToken: token);
        await HasPermission(userId, taskList.Id, token);


        return await _taskListRepository.UpdateAsync(taskList, token);
    }

    public async Task<bool> DeleteByIdAsync(Guid userId, Guid listId, CancellationToken token)
    {
        await EnsureUserExistsAsync(userId, token);
        await EnsureTaskListExistsAsync(listId, token);
        await HasOwnerPermission(userId, listId, token);

        return await _taskListRepository.DeleteByIdAsync(listId, token);
    }

    public async Task<TaskList?> GetByListIdAsync(Guid userId, Guid listId, CancellationToken token)
    {
        await EnsureUserExistsAsync(userId, token);
        await EnsureTaskListExistsAsync(listId, token);
        await HasPermission(userId, listId, token);

        var taskList = await _taskListRepository.GetByListIdAsync(listId, token);

        return taskList;
    }


    public async Task<PagedResult<TaskList>?> GetAllAsync(Guid userId, PageOptions options, CancellationToken token)
    {
        await _pageOptionsValidator.ValidateAndThrowAsync(options, cancellationToken: token);
        await EnsureUserExistsAsync(userId, token);

        return await _taskListRepository.GetAllAsync(userId, options, token);
    }

    public async Task<bool> CreateConnectionAsync(Guid listId, Guid ownerId, Guid otherUserId, CancellationToken token)
    {
        await EnsureUserExistsAsync(ownerId, token);
        await EnsureTaskListExistsAsync(listId, token);
        await HasPermission(ownerId, listId, token);
        var otherUser = await _userRepository.GetByIdAsync(otherUserId, token);
        if (otherUser is null)
        {
            throw new UserNotFoundException();
        }
        var connections = await _taskListRepository.GetAllConnectionsAsync(listId, token);
        if (connections.Any(u => u.Id == otherUserId))
        {
            throw new ConnectionAlreadyExistsException();
        }

        return await _taskListRepository.CreateConnectionAsync(listId, otherUser, token);
    }

    public async Task<List<User>> GetAllConnectionsAsync(Guid userId, Guid listId, CancellationToken token)
    {
        await EnsureUserExistsAsync(userId, token);
        await EnsureTaskListExistsAsync(listId, token);
        await HasPermission(userId, listId, token);

        return await _taskListRepository.GetAllConnectionsAsync(listId, token);
    }

    public async Task<bool> DeleteConnectionsAsync(Guid userId, Guid listId, Guid userIdToDelete, CancellationToken token)
    {
        await EnsureTaskListExistsAsync(listId, token);
        var taskList = await _taskListRepository.GetByListIdAsync(listId, token);
        await EnsureUserExistsAsync(userIdToDelete, token);
        if (taskList!.OwnerId == userIdToDelete)
        {
            throw new OwnerConnectionDeleteException();
        }
        await EnsureUserExistsAsync(userId, token);
        await HasPermission(userId, listId, token);

        if (taskList.ConnectedUsers.All(x => x.Id != userIdToDelete))
        {
            throw new ConnectionNotFoundException();
        }
        
        return await _taskListRepository.DeleteConnectionsAsync(listId, userIdToDelete, token);
    }

    private async Task HasPermission(Guid userId, Guid listId, CancellationToken token)
    {
        var taskList = await _taskListRepository.GetByListIdAsync(listId, token);
        if (taskList!.ConnectedUsers == null || taskList.ConnectedUsers.All(u => u.Id != userId))
        {
            throw new NoPermissionException();
        }
    }

    private async Task HasOwnerPermission(Guid userId, Guid listId, CancellationToken token)
    {
        var taskList = await _taskListRepository.GetByListIdAsync(listId, token);
        if (taskList!.OwnerId != userId)
        {
            throw new NoPermissionException();
        }
    }

    private async Task EnsureTaskListExistsAsync(Guid listId, CancellationToken token)
    {
        var taskListExists = await _taskListRepository.ExistsByIdAsync(listId, token);
        if (!taskListExists)
        {
            throw new TaskListNotFoundException();
        }
    }

    private async Task EnsureUserExistsAsync(Guid userId, CancellationToken token)
    {
        var userExists = await _userRepository.ExistsByIdAsync(userId, token);
        if (!userExists)
        {
            throw new UserNotFoundException();
        }
    }

    private async Task EnsureConnectedUserExistsAsync(Guid userId, CancellationToken token)
    {
        var userExists = await _userRepository.ExistsByIdAsync(userId, token);
        if (!userExists)
        {
            throw new ConnectedUserNotFoundException();
        }
    }
}