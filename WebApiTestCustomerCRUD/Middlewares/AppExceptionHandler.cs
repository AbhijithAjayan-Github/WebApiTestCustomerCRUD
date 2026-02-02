using Microsoft.AspNetCore.Diagnostics;

namespace WebApiTestCustomerCRUD.Middlewares
{
    public class AppExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var statusCode = exception switch
            {
                ArgumentException => StatusCodes.Status400BadRequest,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };

            var errorMessage = new
            {
                StatusCode = statusCode,
                Message = exception switch
                {
                    ArgumentException => "Invalid argument provided.",
                    KeyNotFoundException => "Resource not found.",
                    UnauthorizedAccessException => "Unauthorized access.",
                    _ => "An unexpected error occurred."
                },
                Details = exception.Message
            };

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsJsonAsync(errorMessage, cancellationToken);
            return true;
        }
    }

}
