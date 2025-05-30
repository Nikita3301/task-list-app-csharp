using Microsoft.AspNetCore.Mvc;
using TaskListApp.Mapping;
using TaskLists.Application.Repositories;
using TaskLists.Application.Services;
using TaskLists.Contracts.Requests;

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
    public async Task<IActionResult> CreateAsync([FromBody] CreateTaskListRequest request)
    {
        var userExists = await _userRepository.ExistsByIdAsync(request.UserId);
        if (!userExists)
        {
            return BadRequest("User not found");
        }
        
        if (request.ListName.Length is < 1 or > 255)
        {
            return BadRequest("List name must have from 1 to 255 characters");
        }
        
        var taskList = request.MapToTaskList();
        
        var taskListCreationResult = await _taskListService.CreateAsync(taskList);
        
     
        if (taskListCreationResult is null)
        {
            return BadRequest("Could not create task list");
        }

        if (request.Tasks?.Count > 0)
        {
            var tasks = request.Tasks.MapToTasks(taskListCreationResult.ListId);
            var taskCreationResult = await _taskItemService.CreateMultipleAsync(tasks, taskListCreationResult.ListId);

            if (!taskCreationResult)
            {
                return BadRequest("Could not create tasks");
            }
            
            taskList.Tasks = tasks;
        }

        var result = taskListCreationResult.MapToTaskListResponse();
        
        return Ok(result);

        // ????
        // return CreatedAtRoute(ApiEndpoints.TaskListEndpoints.GetByListId, new { listId = taskList.ListId }, taskList);
    }

    [HttpPut(ApiEndpoints.TaskListEndpoints.Update)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid listId, [FromBody] UpdateTaskListRequest request)
    {
        var userExists = await _userRepository.ExistsByIdAsync(request.UserId);
        if (!userExists)
        {
            return BadRequest("User not found");
        }
        
        var taskList = request.MapToTaskList(listId);
        var result = await _taskListService.UpdateAsync(taskList);

        return Ok(result);
    }

    [HttpPut(ApiEndpoints.TaskListEndpoints.UpdateFull)]
    public async Task<IActionResult> UpdateFullAsync([FromRoute] Guid listId, [FromBody] UpdateFullTaskListRequest request)
    {
        
        var userExists = await _userRepository.ExistsByIdAsync(request.UserId);
        if (!userExists)
        {
            return BadRequest("User not found");
        }
        var taskById = await _taskListService.GetByListIdAsync(listId);
     
        if (taskById is null)
        {
            return NotFound();
        }
        var taskList = request.MapToTaskListAndTasks(listId, taskById.CreatedAt);
        var result = await _taskListService.UpdateFullAsync(taskList);

        return Ok(result);
    }

    [HttpGet(ApiEndpoints.TaskListEndpoints.GetByUserId)]
    public async Task<IActionResult> GetByUserIdAsync([FromRoute] Guid userId)
    {
        var result = await _taskListService.GetByUserIdAsync(userId);

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
        var userExists = await _userRepository.ExistsByIdAsync(request.UserId);
        if (!userExists)
        {
            return BadRequest("User not found");
        }
        
        var result = await _taskListService.GetByListIdAsync(listId);
        return Ok(result);
    }
}