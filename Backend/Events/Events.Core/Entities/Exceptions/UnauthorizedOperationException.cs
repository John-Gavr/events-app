namespace Events.Core.Entities.Exceptions;

public class UnauthorizedOperationException : Exception
{
    public UnauthorizedOperationException()
        : base("You are not authorized to perform this operation.")
    {
    }

    public UnauthorizedOperationException(string message)
        : base(message)
    {
    }
}