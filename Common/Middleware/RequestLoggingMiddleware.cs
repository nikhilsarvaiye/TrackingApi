namespace Common.Middleware
{
    using Common;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestLoggingMiddleware>();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(httpContext, exception).ConfigureAwait(false);
            }
            finally
            {
                _logger.LogInformation(
                    "Request {method} {url} => {statusCode}",
                    httpContext.Request?.Method,
                    httpContext.Request?.Path.Value,
                    httpContext.Response?.StatusCode);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            var applicationError = ApplicationError.From(exception);

            _logger.LogError(applicationError, applicationError.Message);

            httpContext.Response.StatusCode = applicationError.StatusCode;
            httpContext.Response.ContentType = "application/json";
            return httpContext.Response.WriteAsync(ApplicationError.From(exception).ToString());
        }
    }
}
