namespace TaskLists.Application.Repositories;
using Models;


public interface ITaskListRepository
{
    Task<TaskList?> CreateAsync(TaskList taskList);
    Task<List<TaskList>?> GetByUserIdAsync(Guid userId);
    Task<TaskList?> GetByListIdAsync(Guid listId);
    Task<Guid> GetOwnerIdAsync(Guid listId);
    Task<bool> UpdateAsync(TaskList taskList);
    Task<bool> DeleteAsync(TaskList taskList);
    Task<bool> ExistsByIdAsync(Guid id);
   
}