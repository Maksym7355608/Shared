namespace MaksiKo.Shared.Common.Models;

public class AjaxResponse
{
    public bool IsSuccess { get; set; }
    public Dictionary<string, string> Errors { get; set; }
    public object Data { get; set; }

    public AjaxResponse(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }

    public AjaxResponse(bool isSuccess, object data): this(isSuccess)
    {
        Data = data;
    }

    public AjaxResponse(bool isSuccess, object data, Dictionary<string, string> errors) : this(isSuccess, data)
    {
        Errors = errors;
    }

    public AjaxResponse(bool isSuccess, Dictionary<string, string> errors) : this(isSuccess)
    {
        Errors = errors;
    }
}