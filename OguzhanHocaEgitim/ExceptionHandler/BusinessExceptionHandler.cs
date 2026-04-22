using Domains.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.ExceptionHandler
{
    public class BusinessExceptionHandler : IExceptionHandler
    {
        public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
            CancellationToken cancellationToken)
        {
            //fast fail
            if (exception is not BusinessException businessException) return ValueTask.FromResult(false);


            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            httpContext.Response.ContentType = "application/json";

            var problemDetails = new ProblemDetails
            {
                Status = 400,
                Title = "Business Exception",
                Detail = exception.Message
            };


            httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return ValueTask.FromResult(true);
        }
    }
}
