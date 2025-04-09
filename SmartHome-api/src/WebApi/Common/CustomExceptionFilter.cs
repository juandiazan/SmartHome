using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Common;

public class CustomExceptionFilter(ILogger<CustomExceptionFilter> logger) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        logger.LogError(
            context.Exception, "Exception occurred: {Message}", context.Exception.Message);

        var error = new ProblemDetails
        {
            Title = context.Exception.Message
        };

        switch (context.Exception)
        {
            case ArgumentNullException:
                error.Status = StatusCodes.Status400BadRequest;
                break;
            case FormatException:
                error.Status = StatusCodes.Status400BadRequest;
                break;
            case ArgumentException:
                error.Status = StatusCodes.Status400BadRequest;
                break;
            case UnauthorizedAccessException:
                error.Status = StatusCodes.Status401Unauthorized;
                break;
            case KeyNotFoundException:
                error.Status = StatusCodes.Status404NotFound;
                break;
            case InvalidOperationException:
                error.Status = StatusCodes.Status409Conflict;
                break;
            default:
                error.Status = StatusCodes.Status500InternalServerError;
                break;
        }

        context.Result = new ObjectResult(error)
        {
            StatusCode = error.Status
        };
    }
}
