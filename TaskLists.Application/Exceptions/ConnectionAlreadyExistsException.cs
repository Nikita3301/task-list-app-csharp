using System.Net;

namespace TaskLists.Application.Exceptions;

public class ConnectionAlreadyExistsException : BaseException
{
    public ConnectionAlreadyExistsException() : base("Connection already exists")
    {
        ErrorCode = HttpStatusCode.Conflict;
    }
}