namespace MaksiKo.Shared.Common.Validation;

public class EntityNotFoundException : Exception
{
    protected string _message = "Entity with id {0} was not found";
    public override string Message => string.Format(_message, MessageParams);
    public object[] MessageParams { get; private set; }

    public EntityNotFoundException(object id)
    {
        MessageParams = [id];
    }
    
    public EntityNotFoundException(string message, params object[] messageParams)
    {
        MessageParams = messageParams;
        _message = message;
    }
    
}