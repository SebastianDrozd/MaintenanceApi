using MaintenanceApi.Exceptions;
using System.Text.Json;

namespace MaintenanceApi.Middleware
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
            catch (LdapServiceUnavailableException ex)
            {
                _logger.LogError(ex, "Ldap service is unavaible");

                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    message = "Authentication service is currently unavailable"
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Unhandled Exception");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    message = "An unexpected error occured"
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
