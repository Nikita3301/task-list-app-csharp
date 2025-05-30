using Microsoft.AspNetCore.Mvc;
using TaskListApp.Mapping;
using TaskLists.Application.Services;
using TaskLists.Contracts.Requests;

namespace TaskListApp.Controllers;

[ApiController]
public class TaskItemController : ControllerBase
{
    private readonly ITaskItemService _taskItemService;

    public TaskItemController(ITaskItemService taskItemService)
    {
        _taskItemService = taskItemService;
    }

    [HttpPost(ApiEndpoints.TaskItemEndpoints.Create)]
    public async Task<IActionResult> CreateAsync([FromRoute] Guid listId, [FromBody] CreateTaskRequest request)
    {
        var task = request.MapToTask(listId);
        var result = await _taskItemService.CreateAsync(task);

        return result ? Ok() : BadRequest();
    }
}