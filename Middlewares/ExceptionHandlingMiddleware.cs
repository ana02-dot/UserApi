using System.Net;
using System.Text.Json;
using System;

namespace UserProfileAPI.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly RequestDelegate _next;


        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILogger<ExceptionHandlingMiddleware> logger)
        {
            try
            {
                logger.LogInformation("შემოვიდა მოთხოვნა");
                await _next(context);
                logger.LogInformation("მოთხოვნა შესრულდა წარმატებით");
            }
            catch (Exception e)
            {
                logger.LogError($"მოხდა დაუმუშავებელი შეცდომა: {e}");

                await HandleExceptionAsync(context, e);
            }
           
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception e)
        {
            context.Response.ContentType = "application/json";

            var statusCode = e switch
            {
                ArgumentException => (int)HttpStatusCode.BadRequest,   // 400
                KeyNotFoundException => (int)HttpStatusCode.NotFound, // 404
                _ => (int)HttpStatusCode.InternalServerError          // 500
            };

            context.Response.StatusCode = statusCode;

            var response = new
            {
                StatusCode = statusCode,
                Message = e.Message,
                Details = e.InnerException?.Message ?? string.Empty
            };

            var jsonResponse = JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(jsonResponse);
        }
    }

}
