using System.Net;

namespace TaskLists.Application.Exceptions;

public class NoPermissionException : BaseException
{
    public NoPermissionException(string message) : base(message)
    {
        ErrorCode = HttpStatusCode.Forbidden;
        IsSuccess = false;
    }
}