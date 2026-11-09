using RentBikeApi.Infrastructure.Observability;
using Serilog.Context;

namespace RentBikeApi.Middleware;

public sealed class CorrelationIdMiddleware(RequestDelegate next)
{
    public const string HeaderName = "X-Correlation-Id";
    private const int MaxLength = 64;

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = ReadFromRequest(context) ?? Guid.NewGuid().ToString("N");

        context.TraceIdentifier = correlationId;
        CorrelationContext.Current = correlationId;

        context.Response.OnStarting(() =>
        {
            context.Response.Headers[HeaderName] = correlationId;
            return Task.CompletedTask;
        });

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            await next(context);
        }
    }

    private static string? ReadFromRequest(HttpContext context)
    {
        var value = context.Request.Headers[HeaderName].ToString();

        if (value.Length is 0 or > MaxLength)
            return null;

        return value.All(c => char.IsAsciiLetterOrDigit(c) || c is '-' or '_' or '.') ? value : null;
    }
}