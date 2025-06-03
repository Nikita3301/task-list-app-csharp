using FluentValidation;
using TaskLists.Application.Models;
using TaskLists.Application.Repositories;
using Task = System.Threading.Tasks.Task;

namespace TaskLists.Application.Services;

public class TaskListService : ITaskListService
{
    private readonly ITaskListRepository _taskListRepository;
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly IUserRepository _userRepository;
    private readonly IValidator<TaskList> _taskListValidator;

    public TaskListService(ITaskListRepository taskListRepository, ITaskItemRepository taskItemRepository,
        IUserRepository userRepository, IValidator<TaskList> taskListValidator)
    {
        _taskListRepository = taskListRepository;
        _taskItemRepository = taskItemRepository;
        _userRepository = userRepository;
        _taskListValidator = taskListValidator;
    }

    public async Task<TaskList?> CreateAsync(Guid userId, TaskList taskList)
    {
        var userExists = await _userRepository.ExistsByIdAsync(userId);
        if (!userExists)
        {
            return null;
        }

        await _taskListValidator.ValidateAndThrowAsync(taskList);
        var taskListCreated = await _taskListRepository.CreateAsync(taskList);

        return taskListCreated;
    }

    public async Task<List<TaskList>?> GetByUserIdAsync(Guid userId, Guid ownerId)
    {
        var userExists = await _userRepository.ExistsByIdAsync(userId);
        if (!userExists)
        {
            return null;
        }

        var taskList = await _taskListRepository.GetByUserIdAsync(ownerId);
        if (taskList is null)
        {
            return [];
        }

        foreach (var taskListItem in taskList)
        {
            taskListItem.Tasks = await _taskItemRepository.GetTasksByListIdAsync(taskListItem.ListId);
        }

        return taskList;
    }

    public async Task<TaskList?> GetByListIdAsync(Guid userId, Guid listId)
    {
        var userExists = await _userRepository.ExistsByIdAsync(userId);
        if (!userExists)
        {
            return null;
        }

        var taskList = await _taskListRepository.GetByListIdAsync(listId);
        if (taskList is null)
        {
            return null;
        }

        taskList.Tasks = await _taskItemRepository.GetTasksByListIdAsync(taskList.ListId);
        return taskList;
    }

    public async Task<TaskList?> UpdateAsync(Guid userId, TaskList taskList)
    {
        var userExists = await _userRepository.ExistsByIdAsync(userId);
        if (!userExists)
        {
            return null;
        }

        await _taskListValidator.ValidateAndThrowAsync(taskList);

        var taskListExists = await _taskListRepository.ExistsByIdAsync(taskList.ListId);

        if (!taskListExists)
        {
            return null;
        }

        await _taskListRepository.UpdateAsync(taskList);

        var tasks = await _taskItemRepository.GetTasksByListIdAsync(taskList.ListId);

        taskList.Tasks = tasks;

        return taskList;
    }

    public async Task<TaskList?> UpdateFullAsync(Guid userId, TaskList taskList)
    {
        var userExists = await _userRepository.ExistsByIdAsync(userId);
        if (!userExists)
        {
            return null;
        }

        await _taskListValidator.ValidateAndThrowAsync(taskList);

        await _taskListRepository.UpdateAsync(taskList);

        // var tasksExists = await _taskItemRepository.ExistsTasksByListIdAsync(taskList.ListId);

        if (taskList.Tasks is not null)
        {
            await _taskItemRepository.UpdateAllTasksByListIdAsync(taskList.Tasks, taskList.ListId);
        }

        return taskList;
    }

    public Task<bool> DeleteAsync(Guid userId, TaskList taskList)
    {
        throw new NotImplementedException();
    }
}