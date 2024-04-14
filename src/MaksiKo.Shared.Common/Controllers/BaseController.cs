using System.Web.Http;
using System.Web.Http.Results;
using AutoMapper;
using MaksiKo.Shared.Common.Models;

namespace MaksiKo.Shared.Common.Controllers;

public class BaseController : ApiController
{
    protected string BearerToken { get; set; }
    protected readonly IMapper Mapper;
    protected bool IsValid => ModelState.IsValid;
    protected Dictionary<string, string> Errors => ModelState.ToDictionary(
        kvp => kvp.Key,
        kvp => string.Join(", ", kvp.Value.Errors.Select(e => e.ErrorMessage))
    );
    
    public BaseController(IMapper mapper)
    {
        Mapper = mapper;
    }
    
    [NonAction]
    public JsonResult<AjaxResponse> GetAjaxResponse(bool isSuccess)
    {
        return Json(new AjaxResponse(isSuccess));
    }
    
    [NonAction]
    public JsonResult<AjaxResponse> GetAjaxResponse(bool isSuccess, object data, Dictionary<string, string> errors)
    {
        return Json(new AjaxResponse(isSuccess, data, errors));
    }
    
    [NonAction]
    public JsonResult<AjaxResponse> GetAjaxResponse(bool isSuccess, string[] errors)
    {
        return Json(new AjaxResponse(isSuccess, errors));
    }
}