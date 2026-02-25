using System.Net;
using System.Text.Json;

namespace API.MidlleWare
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
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
            catch (Exception exception)
            {
                _logger.LogError(exception, "Ocorreu uma exceção não tratada: {Message}", exception.Message);
                await HandleExceptionAsync(context, exception);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new
            {
                erro = "",
                tipo = "",
                statusCode = 500
            };

            switch (exception)
            {
                case Domain.Validators.ValidationException validationEx:
                    response = new
                    {
                        erro = validationEx.Message,
                        tipo = "Validação",
                        statusCode = 400
                    };
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case FluentValidation.ValidationException fluentValidationEx:
                    var errors = string.Join("; ", fluentValidationEx.Errors.Select(e => e.ErrorMessage));
                    response = new
                    {
                        erro = errors,
                        tipo = "Validação",
                        statusCode = 400
                    };
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case UnauthorizedAccessException unauthorizedEx:
                    response = new
                    {
                        erro = unauthorizedEx.Message,
                        tipo = "Não Autorizado",
                        statusCode = 401
                    };
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;

                case InvalidOperationException invalidOpEx:
                    response = new
                    {
                        erro = invalidOpEx.Message,
                        tipo = "Conflito",
                        statusCode = 409
                    };
                    context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    break;

                case ArgumentException argEx:
                    response = new
                    {
                        erro = argEx.Message,
                        tipo = "Argumento Inválido",
                        statusCode = 400
                    };
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case KeyNotFoundException notFoundEx:
                    response = new
                    {
                        erro = notFoundEx.Message,
                        tipo = "Não Encontrado",
                        statusCode = 404
                    };
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                default:
                    response = new
                    {
                        erro = "Erro interno do servidor",
                        tipo = "Erro Interno",
                        statusCode = 500
                    };
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
