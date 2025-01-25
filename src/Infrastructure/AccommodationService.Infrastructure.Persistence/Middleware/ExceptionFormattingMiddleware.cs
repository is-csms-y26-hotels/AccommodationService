using Microsoft.AspNetCore.Http;

namespace AccommodationService.Infrastructure.Persistence.Middleware;

public class ExceptionFormattingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            string message = $"""
                              Middleware Exception occured while processing request, type = {e.GetType().Name}, message = {e.Message}";
                              """;

            context.Response.StatusCode = e.Message switch
            {
                var msg when msg.Contains("not found", StringComparison.OrdinalIgnoreCase) => StatusCodes.Status404NotFound,
                var msg when msg.Contains("validation error", StringComparison.OrdinalIgnoreCase) => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError,
            };
            await context.Response.WriteAsync(message);
        }
    }
}