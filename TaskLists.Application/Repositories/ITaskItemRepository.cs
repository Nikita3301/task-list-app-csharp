using TaskLists.Application.Models;

namespace TaskLists.Application.Repositories;

public interface ITaskItemRepository
{
    Task<bool> CreateAsync(TaskItem task);
    Task<bool> CreateMultipleAsync(List<TaskItem> tasks, Guid listId);
    
    Task<List<TaskItem>> GetTasksByListIdAsync(Guid listId);
    Task<bool> ExistsTasksByListIdAsync(Guid listId);
    
    Task<bool> UpdateAllTasksByListIdAsync(List<TaskItem> tasks, Guid listId);
}