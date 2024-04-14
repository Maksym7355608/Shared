using System.Net;
using System.Web.Http.Filters;
using MaksiKo.Shared.Common.Validation;

namespace MaksiKo.Shared.Common.Attributes;

public class HandleExceptionAttribute : ExceptionFilterAttribute
{
    public override void OnException(HttpActionExecutedContext context)
    {
        // Ловимо винятки та обробляємо їх
        if (context.Exception == null) return;
        
        // Отримуємо повідомлення про помилку
        var errorMessage = context.Exception.Message;
            
        switch (context.Exception)
        {
            case EntityNotFoundException notFoundException:
                context.ActionContext.ModelState.AddModelError("", notFoundException.Message);
                break;
            case EntityExistingException existingException:
                context.ActionContext.ModelState.AddModelError("", existingException.Message);
                break;
            default:
                context.ActionContext.ModelState.AddModelError("", errorMessage);
                break;
        }

        context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
    }
}