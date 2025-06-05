using System.Net;

namespace TaskLists.Application.Exceptions;

public class ConnectionNotFoundException : BaseException
{
    public ConnectionNotFoundException() : base("Connection not found")
    {
        ErrorCode = HttpStatusCode.NotFound;
    }
}