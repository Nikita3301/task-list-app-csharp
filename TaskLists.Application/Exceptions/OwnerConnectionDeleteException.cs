using System.Net;

namespace TaskLists.Application.Exceptions;

public class OwnerConnectionDeleteException : BaseException
{
    public OwnerConnectionDeleteException() : base("Couldn't delete owner connection")
    {
        ErrorCode = HttpStatusCode.BadRequest;
    
    }
}