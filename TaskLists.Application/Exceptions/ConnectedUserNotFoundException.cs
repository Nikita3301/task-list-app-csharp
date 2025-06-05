using System.Net;

namespace TaskLists.Application.Exceptions;

public class ConnectedUserNotFoundException : BaseException
{
    public ConnectedUserNotFoundException() : base("Connected user not found")
    {
        ErrorCode = HttpStatusCode.NotFound;
    }
}