using TaskLists.Application.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskLists.Application.Services;

public interface ITaskListService
{
    Task<TaskList?> CreateAsync(Guid userId,TaskList taskList);
    Task<List<TaskList>?> GetByUserIdAsync(Guid userId,Guid ownerId);
    Task<TaskList?> GetByListIdAsync(Guid userId,Guid listId);
    Task<TaskList?> UpdateAsync(Guid userId,TaskList taskList);
    Task<TaskList?> UpdateFullAsync(Guid userId,TaskList taskList);
    Task<bool> DeleteAsync(Guid userId,TaskList taskList);
}