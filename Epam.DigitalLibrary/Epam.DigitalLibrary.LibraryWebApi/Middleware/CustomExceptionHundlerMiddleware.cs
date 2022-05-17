using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Epam.DigitalLibrary.CustomExeptions;

namespace Epam.DigitalLibrary.LibraryWebApi.Middleware
{
    public class CustomExceptionHundlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHundlerMiddleware> _logger;

        public CustomExceptionHundlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHundlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }

            catch (Exception e)
            {
                await HandleExceptionAsync(context, e);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = string.Empty;

            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(validationException.ValidationResult);
                    _logger.LogWarning(3, $" Validation exception | user: {context.User.Identity.Name ?? "Guest"}");
                    break;

                case BusinessLogicException businessLogicException:
                    code = HttpStatusCode.InternalServerError;
                    result = JsonSerializer.Serialize(businessLogicException.Message);
                    _logger.LogError(4, $" Business logic exception | user: {context.User.Identity.Name ?? "Guest"} | source: {exception.StackTrace}");
                    break;

                case DataAccessException dataAccessException:
                    code = HttpStatusCode.InternalServerError;
                    result = JsonSerializer.Serialize(dataAccessException.Message);
                    _logger.LogError(4, $" Data access logic exception | user: {context.User.Identity.Name ?? "Guest"} | source: {exception.StackTrace}");
                    break;

                case Exception unhandledException:
                    code = HttpStatusCode.InternalServerError;
                    result = JsonSerializer.Serialize(unhandledException.Message);
                    _logger.LogError(4, $" Unhandled exception | user: {context.User.Identity.Name ?? "Guest"} | source: {exception.StackTrace}");
                    break;

                default:
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            if (string.IsNullOrEmpty(result))
            {
                result = JsonSerializer.Serialize( new { error = exception.Message });
            }

            return context.Response.WriteAsync(result);
        }
    }
}
