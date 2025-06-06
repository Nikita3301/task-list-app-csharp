namespace TaskLists.Application.Repositories;
using Models;


public interface ITaskListRepository
{
    Task<TaskList?> CreateAsync(TaskList taskList, CancellationToken token);
    Task<TaskList?> UpdateAsync(TaskList taskList, CancellationToken token);
    Task<bool> DeleteByIdAsync(Guid listId, CancellationToken token);
    Task<TaskList?> GetByListIdAsync(Guid listId, CancellationToken token);
    Task<PagedResult<TaskList>?> GetAllAsync(Guid userId, PageOptions options, CancellationToken token);
    Task<bool> CreateConnectionAsync(Guid taskListId, User otherUser, CancellationToken token);
    Task<List<User>> GetAllConnectionsAsync(Guid listId, CancellationToken token);
    Task<bool> DeleteConnectionsAsync(Guid listId, Guid userIdToDelete, CancellationToken token);
    
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken token);
   
}