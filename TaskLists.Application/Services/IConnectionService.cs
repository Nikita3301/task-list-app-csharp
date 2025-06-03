using TaskLists.Application.Models;

namespace TaskLists.Application.Services;

public interface IConnectionService
{
    Task<bool> CreateAsync(Guid userId, Guid taskListId, Guid otherUserId);
    Task<TaskListConnection?> GetAsync(Guid userId, Guid taskListId);
    Task<bool> DeleteAsync(Guid userId, Guid taskListId, Guid userIdToDelete);
}