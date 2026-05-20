using Microsoft.Extensions.Primitives;

namespace Presentation.API.Middleware
{
    public class LogScopeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogScopeMiddleware> _logger;

        public LogScopeMiddleware(RequestDelegate next, ILogger<LogScopeMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers.TryGetValue("Authorization", out var authHeader)
                ? authHeader.ToString()
                : string.Empty;
            var userId = 100;
            var tenantId = 200;


            using (_logger.BeginScope("UserId:{UserId} TenantId:{TenantId}", userId, tenantId))
            {
                await _next(context);
            }
        }
    }
}