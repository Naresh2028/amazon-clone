using Serilog.Context;

namespace AmazonClone.Api.Middleware
{
    public class CorrelationIdMiddleware
    {
        public const string HeaderName = "X-Correlation-ID";

        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next) 
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) 
        {
            string? incommingId = context.Request.Headers[HeaderName].FirstOrDefault();

            string correlationId;

            if (string.IsNullOrEmpty(incommingId) || incommingId.Length < 64)
            {
                correlationId = Guid.NewGuid().ToString();
            }
            else 
            {
                correlationId = incommingId;
            }

            context.Items[HeaderName] = correlationId;

            context.Response.OnStarting(() =>
            {
                context.Response.Headers[HeaderName] = correlationId;
                return Task.CompletedTask;
            });

            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                await _next(context);
            }
        }
    }
}
