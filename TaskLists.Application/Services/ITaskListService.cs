using TaskLists.Application.Models;

namespace TaskLists.Application.Services;

public interface ITaskListService
{
    Task<TaskList?> CreateAsync(Guid userId, TaskList taskList);
    Task<TaskList?> UpdateAsync(Guid userId, TaskList taskList);
    Task<bool> DeleteByIdAsync(Guid userId, Guid listId);
    Task<TaskList?> GetByListIdAsync(Guid userId, Guid listId);
    Task<PagedResult<TaskList>?> GetAllAsync(Guid userId, PageOptions options);
    Task<bool> CreateConnectionAsync(Guid listId, Guid ownerId, Guid otherUserId);
    Task<List<User>> GetAllConnectionsAsync(Guid userId, Guid listId);
    Task<bool> DeleteConnectionsAsync(Guid userId, Guid listId, Guid userIdToDelete);
}