using System.Text.Json;

namespace SportsClubPlatform.Api.Middleware
{
    /// <summary>
    /// Global exception middleware returning JSON error payloads.
    /// </summary>
    public sealed class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                string payload = JsonSerializer.Serialize(new
                {
                    message = exception.Message
                });

                await context.Response.WriteAsync(payload);
            }
        }
    }
}
