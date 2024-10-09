namespace Events.Core.Entities.Exceptions;

public class EventAlredyExistException : Exception
{
    public EventAlredyExistException(object eventKey) : base($"Event with key '{eventKey}' is already exist.")
    {

    }
}