using System.Net;

namespace TaskLists.Application.Exceptions;

public class BaseException : Exception
{
    public BaseException(string message) : base(message)
    {
    }

    public HttpStatusCode ErrorCode { get; init; }
    public bool IsSuccess { get; init; }
}