using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ContactSystem.Server.ActionHelpers;

public class AppExceptionHendler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        (int statusCode, string errorMessage) = exception switch
        {
            NullReferenceException nullReferenceException => (400, nullReferenceException.Message),
            ArgumentException argumentException => (400, argumentException.Message),
            Exception defaultException => (400, defaultException.Message),
            _ => default
        };

        if (statusCode == default)
        {
            return false;
        }

        var problemDetails = new ProblemDetails()
        {
            Status = statusCode,
            Detail = errorMessage,
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(errorMessage);

        return true;
    }
}
