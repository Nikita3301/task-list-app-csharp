using System.Net;

namespace TaskLists.Application.Exceptions;

public class TaskListCreationException :BaseException
{
    public TaskListCreationException() : base("TaskList creation error")
    {
        ErrorCode = HttpStatusCode.BadRequest;
    }
}