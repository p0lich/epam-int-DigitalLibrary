using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryWebApi.Middleware
{
    public static class AuthorizationMiddlewareExtension
    {
        public static IApplicationBuilder UseCustomAuthorizationDeniedHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthorizationMiddleware>();
        }
    }
}
