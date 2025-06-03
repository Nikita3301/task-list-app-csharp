using Microsoft.AspNetCore.Mvc;
using TaskLists.Application.Services;
using TaskLists.Contracts.Requests;

namespace TaskListApp.Controllers;

[ApiController]
public class ConnectionController : ControllerBase
{
    private readonly IConnectionService _connectionService;

    public ConnectionController(IConnectionService connectionService)
    {
        _connectionService = connectionService;
    }

    [HttpPost(ApiEndpoints.TaskListConnectionsEndpoints.Create)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateConnectionRequest request)
    {
        var result = await _connectionService.CreateAsync(request.UserId, request.ListId, request.OtherUserId);
        if (!result)
        {
            return BadRequest("Could not create connection");
        }
        
        return Ok("Connection created");
    }
    
    
    [HttpGet(ApiEndpoints.TaskListConnectionsEndpoints.Get)]
    public async Task<IActionResult> GetAsync([FromRoute] Guid listId, [FromQuery] Guid userId)
    {
        var result = await _connectionService.GetAsync(userId, listId);
        if (result is null)
        {
            return BadRequest("Could not get connection");
        }
        
        return Ok(result);
    }
    
    [HttpDelete(ApiEndpoints.TaskListConnectionsEndpoints.Delete)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid listId, [FromBody] DeleteConnectionRequest request)
    {
        var result = await _connectionService.DeleteAsync(request.UserId, listId, request.UserIdToDelete);
        if (!result)
        {
            return BadRequest("Could not delete connection");
        }
        
        return Ok("Connection removed");
    }
    
}