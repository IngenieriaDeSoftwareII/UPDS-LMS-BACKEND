using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;

namespace Api.Middleware;

public class UserActivityMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IMemoryCache cache)
    {
        // Solo rastrear actividad de usuarios autenticados
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                var cacheKey = $"user_activity:{userId}";
                cache.Set(cacheKey, DateTime.UtcNow, TimeSpan.FromMinutes(30));
            }
        }

        await next(context);
    }
}
