using TaskLists.Application.Models;

namespace TaskLists.Application.Repositories;

public interface IConnectionRepository
{
    Task<bool> CreateAsync(Guid taskListId, Guid ownerId, Guid otherUserId);
    Task<bool> UpdateAsync(Guid taskListId, Guid otherUserId);
    Task<TaskListConnection?> GetAsync(Guid taskListId);
    Task<bool> DeleteAsync(Guid taskListId, Guid userIdToDelete);
    Task<bool> HasUserPermissionAsync(Guid userId, Guid taskListId);
    Task<bool> ConnectionExistAsync(Guid taskListId);
    Task<bool> HasConnectionWithUserAsync(Guid userId, Guid taskListId);
}