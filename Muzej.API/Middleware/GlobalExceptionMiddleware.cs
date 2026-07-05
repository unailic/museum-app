using FluentValidation;
using System.Text.Json;

namespace Muzej.API.Middleware
{
    public class GlobalExceptionMiddleware
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
            catch (ValidationException ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var greske = ex.Errors.Select(e => new { polje = e.PropertyName, poruka = e.ErrorMessage });
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { greske }));
            }
            catch (InvalidOperationException ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { poruka = ex.Message }));
            }
        }
    }
}