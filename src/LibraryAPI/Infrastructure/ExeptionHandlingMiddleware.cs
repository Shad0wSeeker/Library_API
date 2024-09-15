using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace LibraryAPI.Infrastructure
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");

                HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
                string message = "An unexpected error occurred.";

                switch (ex)
                {
                    case UnauthorizedAccessException _:
                        statusCode = HttpStatusCode.Forbidden;
                        message = "You do not have permission to access this resource.";
                        break;

                    case ArgumentException _:
                        statusCode = HttpStatusCode.BadRequest;
                        message = "Invalid input.";
                        break;

                    case NotImplementedException _:
                        statusCode = HttpStatusCode.NotImplemented;
                        message = "This feature is not implemented.";
                        break;

                    case KeyNotFoundException _:
                        statusCode = HttpStatusCode.NotFound;
                        message = "Resource not found.";
                        break;

                    
                }


                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)statusCode;

                var errorResponse = new
                {
                    error = new
                    {
                        message,
                        details = ex.Message
                    }
                };

                var jsonResponse = JsonSerializer.Serialize(errorResponse);
                await response.WriteAsync(jsonResponse);
            }
        }
    }
}
