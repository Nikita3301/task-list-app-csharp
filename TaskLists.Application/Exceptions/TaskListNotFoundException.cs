using System.Net;

namespace TaskLists.Application.Exceptions;

public class TaskListNotFoundException : BaseException
{
    public TaskListNotFoundException(string message) : base(message)
    {
        ErrorCode = HttpStatusCode.NotFound;
        IsSuccess = false;
    }
}