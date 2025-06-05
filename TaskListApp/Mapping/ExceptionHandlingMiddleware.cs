using TaskLists.Application.Exceptions;
using TaskLists.Contracts.Responses;

namespace TaskListApp.Mapping;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BaseException e)
        {
            context.Response.StatusCode = (int)e.ErrorCode;
            var response = new ErrorResponse()
            {
                Message = e.Message,
            };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
    
}