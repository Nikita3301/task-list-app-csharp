using System.Net;

namespace TaskLists.Application.Exceptions;

public class NoPermissionException : BaseException
{
    public NoPermissionException() : base("User have no permission")
    {
        ErrorCode = HttpStatusCode.Forbidden;
    
    }
}