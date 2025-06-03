using Microsoft.AspNetCore.Mvc;
using TaskListApp.Mapping;
using TaskLists.Application.Repositories;
using TaskLists.Application.Services;
using TaskLists.Contracts.Requests;
using TaskLists.Contracts.Responses;

namespace TaskListApp.Controllers;

[ApiController]
public class TaskListController : ControllerBase
{
    private readonly ITaskListService _taskListService;
    private readonly ITaskItemService _taskItemService;
    private readonly IUserRepository _userRepository;


    public TaskListController(ITaskListService taskListService, ITaskItemService taskItemService, IUserRepository userRepository)
    {
        _taskListService = taskListService;
        _taskItemService = taskItemService;
        _userRepository = userRepository;
    }

    [HttpPost(ApiEndpoints.TaskListEndpoints.Create)]
    [ProducesResponseType(typeof(TaskListResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateTaskListRequest request)
    {
        var taskList = request.MapToTaskList();
        
        if (request.Tasks?.Count > 0)
        {
            var tasks = request.Tasks.MapToTasks(taskList.ListId);
            taskList.Tasks = tasks;
        }
        
        var taskListCreationResult = await _taskListService.CreateAsync(request.UserId, taskList);
        if (taskListCreationResult is null)
        {
            return BadRequest("Could not create task list");
        }
        
        var result = taskListCreationResult.MapToTaskListResponse();
        
        return Ok(result);

        
        // ????
        // return CreatedAtAction(nameof(GetByListIdAsync), new { listId = taskList.ListId }, new GetTaskListRequest
        // {
        //     UserId = Guid.Parse((ReadOnlySpan<char>)"3fa85f64-5717-4562-b3fc-2c963f66afa6")
        // });
    }

    [HttpPut(ApiEndpoints.TaskListEndpoints.Update)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid listId, [FromBody] UpdateTaskListRequest request)
    {
        var taskList = request.MapToTaskList(listId);
        var result = await _taskListService.UpdateAsync(request.UserId, taskList);

        return Ok(result);
    }

    [HttpPut(ApiEndpoints.TaskListEndpoints.UpdateFull)]
    public async Task<IActionResult> UpdateFullAsync([FromRoute] Guid listId, [FromBody] UpdateFullTaskListRequest request)
    {
        var taskById = await _taskListService.GetByListIdAsync(request.UserId, listId);
     
        if (taskById is null)
        {
            return NotFound();
        }
        var taskList = request.MapToTaskListAndTasks(listId, taskById.CreatedAt);
        var result = await _taskListService.UpdateFullAsync(request.UserId, taskList);

        return Ok(result);
    }

    [HttpGet(ApiEndpoints.TaskListEndpoints.GetByUserId)]
    public async Task<IActionResult> GetByUserIdAsync([FromRoute] Guid ownerId, [FromQuery] Guid userId)
    {
        var result = await _taskListService.GetByUserIdAsync(userId, ownerId);

        return result switch
        {
            null => NotFound("User with this id does not exist"),
            [] => NoContent(),
            _ => Ok(result)
        };
    }
    
    [HttpGet(ApiEndpoints.TaskListEndpoints.GetByListId)]
    public async Task<IActionResult> GetByListIdAsync([FromRoute] Guid listId, [FromBody] GetTaskListRequest request)
    {
        var result = await _taskListService.GetByListIdAsync(request.UserId, listId);
        return Ok(result);
    }
    
}