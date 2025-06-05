using System.Net;

namespace TaskLists.Application.Exceptions;

public class UserNotFoundException : BaseException
{
    public UserNotFoundException() : base("User not found")
    {
        ErrorCode = HttpStatusCode.NotFound;
    }
}