
using ImageSystem.Core;
using ImageSystem.Web.Common.Exceptions;
using ImageSystem.Web.Common.Models;
using System.Net;

namespace ImageSystem.Web.Common.Middlewares;

public class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public CustomExceptionHandlerMiddleware(RequestDelegate next) =>
        _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var logger = context.RequestServices.GetService<ILogger<CustomExceptionHandlerMiddleware>>();
        ErrorResponse result;
        switch (exception)
        {
            case AccessException accessException:
                result = ErrorResponse.Create((int)HttpStatusCode.Forbidden, accessException.Message);
                break;
            case ArgumentException argumentException:
                result = ErrorResponse.Create((int)HttpStatusCode.BadRequest, argumentException.Message);
                break;
            case ImageLoadingException imageLoading:
                result = ErrorResponse.Create((int)HttpStatusCode.BadRequest, imageLoading.Message);
                break;
            case NotFoundException notFound:
                result = ErrorResponse.Create((int)HttpStatusCode.NotFound, notFound.Message);
                break;
            default:
                result = ErrorResponse.Create((int)HttpStatusCode.InternalServerError, exception.Message);
                logger.LogError($"{result.StatusCode}, {exception.Message}");
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)result.StatusCode;
        return context.Response.WriteAsJsonAsync(result);
    }
}
