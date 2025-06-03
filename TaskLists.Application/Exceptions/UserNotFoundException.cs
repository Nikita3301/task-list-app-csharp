using System.Net;

namespace TaskLists.Application.Exceptions;

public class UserNotFoundException : BaseException
{
    public UserNotFoundException(string message) : base(message)
    {
        ErrorCode = HttpStatusCode.NotFound;
        IsSuccess = false;
    }
}