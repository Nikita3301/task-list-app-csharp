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

    public TaskListController(ITaskListService taskListService)
    {
        _taskListService = taskListService;
    }


    // Створити новий список задач
    [HttpPost(ApiEndpoints.TaskListEndpoints.Create)]
    [ProducesResponseType(typeof(TaskListResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateTaskListRequest request, CancellationToken token)
    {
        var taskList = request.MapToTaskList();

        var taskListCreationResult = await _taskListService.CreateAsync(request.UserId, taskList, token);
        var result = taskListCreationResult?.MapToTaskListResponse();

        return Created(
            ApiEndpoints.TaskListEndpoints.GetByListId,
            result
        );

        // ????
        // return CreatedAtAction(nameof(GetByListIdAsync), new { listId = taskList.Id }, result);
    }


    // Змінити існуючий список задач
    [HttpPut(ApiEndpoints.TaskListEndpoints.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid listId, [FromBody] UpdateTaskListRequest request,
        CancellationToken token)
    {
        var taskList = request.MapToTaskList(listId);
        var result = await _taskListService.UpdateAsync(request.UserId, taskList, token);

        return Ok(result);
    }

    // Видалити існуючий список задач
    [HttpDelete(ApiEndpoints.TaskListEndpoints.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid listId, [FromBody] DeleteTaskListRequest request,
        CancellationToken token)
    {
        var result = await _taskListService.DeleteByIdAsync(request.UserId, listId, token);
        return result ? Ok() : BadRequest();
    }

    // Отримати один існуючий список задач
    [HttpGet(ApiEndpoints.TaskListEndpoints.GetByListId)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByListIdAsync([FromRoute] Guid listId, [FromQuery] GetTaskListRequest request,
        CancellationToken token)
    {
        var result = await _taskListService.GetByListIdAsync(request.UserId, listId, token);
        return Ok(result);
    }

    // Отримати список списків задач 
    [HttpGet(ApiEndpoints.TaskListEndpoints.GetAll)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllTaskListsRequest request, CancellationToken token)
    {
        var options = request.MapToOptions();

        var result = await _taskListService.GetAllAsync(request.UserId, options, token);
        var response = result?.MapToPagedResponse();

        return Ok(response);
    }

    // Додати зв’язок одного списку задач з вказаним користувачем
    [HttpPost(ApiEndpoints.TaskListConnectionsEndpoints.CreateConnection)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateConnectionAsync([FromRoute] Guid listId,
        [FromBody] CreateConnectionRequest request, CancellationToken token)
    {
        var result = await _taskListService.CreateConnectionAsync(listId, request.UserId, request.OtherUserId, token);
        return result ? Created() : BadRequest();
    }


    // Отримати зв’язки одного списку задач з користувачами
    [HttpGet(ApiEndpoints.TaskListConnectionsEndpoints.GetAllConnections)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllConnections([FromQuery] GetAllConnectionsRequest request,
        CancellationToken token)
    {
        var result = await _taskListService.GetAllConnectionsAsync(request.UserId, request.ListId, token);

        return Ok(result);
    }

    // Прибрати зв’язок одного списку задач з вказаним користувачем
    [HttpDelete(ApiEndpoints.TaskListConnectionsEndpoints.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteConnectionAsync([FromRoute] Guid listId,
        [FromBody] DeleteConnectionRequest request, CancellationToken token)
    {
        var result =
            await _taskListService.DeleteConnectionsAsync(request.UserId, listId, request.UserIdToDelete, token);
        return result ? Ok() : BadRequest();
    }
}