
using TaskLists.Application.Models;

namespace TaskLists.Application.Services;

public interface ITaskItemService
{
    Task<bool> CreateAsync(TaskItem task);
    Task<bool> CreateMultipleAsync(List<TaskItem> tasks, Guid listId);
}