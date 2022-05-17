using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryWebApi.Middleware
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthorizationMiddleware> _logger;

        public AuthorizationMiddleware(RequestDelegate next, ILogger<AuthorizationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                await context.Response.WriteAsync(JsonSerializer.Serialize("Unauthenticate access attempt"));
                _logger.LogWarning(3, "Unauthenticate access attempt");
            }

            if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
            {
                await context.Response.WriteAsync(JsonSerializer.Serialize("Unauthorize access attempt"));
                _logger.LogWarning(3, $"Attempt to acces without required rights | User: {context.User.Identity.Name ?? "Guest"}");
            }
        }
    }
}
