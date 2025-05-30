using TaskLists.Application.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskLists.Application.Services;

public interface ITaskListService
{
    Task<TaskList?> CreateAsync(TaskList taskList);
    Task<List<TaskList>?> GetByUserIdAsync(Guid userId);
    Task<TaskList?> GetByListIdAsync(Guid listId);
    Task<TaskList?> UpdateAsync(TaskList taskList);
    Task<TaskList?> UpdateFullAsync(TaskList taskList);
    Task<bool> DeleteAsync(TaskList taskList);
}