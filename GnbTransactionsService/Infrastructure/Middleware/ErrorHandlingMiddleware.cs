using GnbTransactionsService.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace GnbTransactionsService.Infrastructure.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ErrorHandlingMiddleware> logger;

        /// <summary>
        /// Pipeline middleware to ensure that unhandled exceptions are properly logged and handled.
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public ErrorHandlingMiddleware(
            RequestDelegate next,
            ILogger<ErrorHandlingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Map exceptions to appropriate HTTP status codes and messages
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        private async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            HttpStatusCode status;
            string message;

            switch (exception)
            {
                case CurrencyConversionException:
                    status = HttpStatusCode.UnprocessableEntity; // 422
                    message = exception.Message;
                    break;

                case FileNotFoundException:
                    status = HttpStatusCode.InternalServerError; //500
                    message = "Required data file not found";
                    break;

                case JsonException:
                    status = HttpStatusCode.BadRequest; //400
                    message = "Invalid JSON data format";
                    break;

                case DataConsistencyException:
                    status = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    break;

                default:
                    status = HttpStatusCode.InternalServerError; //500
                    message = "An unexpected error occurred";
                    break;
            }

            // Completely backend log with stack trace (to debug)
            logger.LogError(
                exception,
                "Unhandled exception while processing request {Method} {Path}",
                context.Request.Method,
                context.Request.Path
            );

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            var response = new
            {
                error = message,
                status = context.Response.StatusCode,
                traceId = context.TraceIdentifier
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response)
            );
        }
    }
}
