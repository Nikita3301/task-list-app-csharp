using Microsoft.AspNetCore.Mvc;
using TaskLists.Application.Services;
using TaskLists.Contracts.Requests;

namespace TaskListApp.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost(ApiEndpoints.UsersEndpoints.Create)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateUserRequest request)
    { 
        var result = await _userService.CreateAsync(request.UserId, request.FullName);
        return result ? Ok() : BadRequest();
    }
}