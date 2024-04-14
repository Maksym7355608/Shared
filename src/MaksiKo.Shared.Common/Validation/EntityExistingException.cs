namespace MaksiKo.Shared.Common.Validation;

public class EntityExistingException : Exception
{
    protected string _message = "Entity with id {0} was exist";
    public override string Message => string.Format(_message, MessageParams);
    public object[] MessageParams { get; private set; }

    public EntityExistingException(object id)
    {
        MessageParams = [id];
    }

    public EntityExistingException(string message, params object[] messageParams)
    {
        MessageParams = messageParams;
        _message = message;
    }
}