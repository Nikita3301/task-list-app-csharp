using System.Net;

namespace TaskLists.Application.Exceptions;

public class TaskListNotFoundException : BaseException
{
    public TaskListNotFoundException() : base("TaskList not found")
    {
        ErrorCode = HttpStatusCode.NotFound;
     
    }
}