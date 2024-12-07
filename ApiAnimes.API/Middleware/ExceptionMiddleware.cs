using System;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using ApiAnimes.Domain.Entities;

namespace ApiAnimes.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleCustomExceptionResponseAsync(context, ex);
            }
        }

        private async Task HandleCustomExceptionResponseAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;

            switch (ex)
            {
                case ArgumentException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case UnauthorizedAccessException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;

                case KeyNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case DbUpdateException:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var response = new ErrorModel(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString());
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }

    // Model de erro (ajustado para refletir o retorno de erro)
    public class ErrorModel
    {
        public int StatusCode { get; }
        public string Message { get; }
        public string StackTrace { get; }

        public ErrorModel(int statusCode, string message, string stackTrace)
        {
            StatusCode = statusCode;
            Message = message;
            StackTrace = stackTrace;
        }
    }
}
