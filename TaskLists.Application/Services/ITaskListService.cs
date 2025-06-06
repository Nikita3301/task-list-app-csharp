using TaskLists.Application.Models;

namespace TaskLists.Application.Services;

public interface ITaskListService
{
    Task<TaskList?> CreateAsync(Guid userId, TaskList taskList, CancellationToken token);
    Task<TaskList?> UpdateAsync(Guid userId, TaskList taskList, CancellationToken token);
    Task<bool> DeleteByIdAsync(Guid userId, Guid listId, CancellationToken token);
    Task<TaskList?> GetByListIdAsync(Guid userId, Guid listId, CancellationToken token);
    Task<PagedResult<TaskList>?> GetAllAsync(Guid userId, PageOptions options, CancellationToken token);
    Task<bool> CreateConnectionAsync(Guid listId, Guid ownerId, Guid otherUserId, CancellationToken token);
    Task<List<User>> GetAllConnectionsAsync(Guid userId, Guid listId, CancellationToken token);
    Task<bool> DeleteConnectionsAsync(Guid userId, Guid listId, Guid userIdToDelete, CancellationToken token);
}