namespace Events.Core.Entities.Exceptions;

public class ParticipationAlredyExistException : Exception
{
    public ParticipationAlredyExistException(object eventKey, object userKey) : base($"User with id '{userKey}' is already registred for event with id '{eventKey}'.")
    {

    }
}