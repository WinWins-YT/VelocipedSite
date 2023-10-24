using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VelocipedSite.DAL.Exceptions;

namespace VelocipedSite.ActionFilters;

public sealed class ExceptionFilterAttribute : Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case InfrastructureException exception:
                HandleBadRequest(context, exception);
                break;
            
            default:
                HandleInternalError(context);
                break;
        }
    }
    
    private static void HandleInternalError(ExceptionContext context)
    {
        var jsonResult = new JsonResult(new ErrorResponse(
            HttpStatusCode.InternalServerError, 
            "Возникла ошибка, уже чиним"))
        {
            StatusCode = (int)HttpStatusCode.InternalServerError
        };
        context.Result = jsonResult;
    }

    private static void HandleBadRequest(ExceptionContext context, Exception exception)
    {
        var jsonResult = new JsonResult(
            new ErrorResponse(
                HttpStatusCode.BadRequest, 
                exception.Message))
        {
            StatusCode = (int)HttpStatusCode.BadRequest
        };

        context.Result = jsonResult;
    }
}