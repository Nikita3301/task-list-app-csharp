namespace TaskLists.Application.Repositories;
using Models;


public interface ITaskListRepository
{
    Task<TaskList?> CreateAsync(TaskList taskList);
    Task<TaskList?> UpdateAsync(TaskList taskList);
    Task<bool> DeleteByIdAsync(Guid listId);
    Task<TaskList?> GetByListIdAsync(Guid listId);
    Task<PagedResult<TaskList>?> GetAllAsync(Guid userId, PageOptions options);
    Task<bool> CreateConnectionAsync(Guid taskListId, User otherUser);
    Task<List<User>> GetAllConnectionsAsync(Guid listId);
    Task<bool> DeleteConnectionsAsync(Guid listId, Guid userIdToDelete);
    
    Task<bool> ExistsByIdAsync(Guid id);
   
}