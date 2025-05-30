using TaskLists.Application.Models;
using TaskLists.Application.Repositories;

namespace TaskLists.Application.Services;

public class TaskItemService : ITaskItemService
{
    private readonly ITaskItemRepository _taskItemRepository;

    public TaskItemService(ITaskItemRepository taskItemRepository)
    {
        _taskItemRepository = taskItemRepository;
    }


    public async Task<bool> CreateAsync(TaskItem task)
    {
        return await _taskItemRepository.CreateAsync(task);
    }

    public async Task<bool> CreateMultipleAsync(List<TaskItem> tasks, Guid listId)
    {
        return await _taskItemRepository.CreateMultipleAsync(tasks, listId);
    }
}