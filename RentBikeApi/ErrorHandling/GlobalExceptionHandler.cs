using Microsoft.AspNetCore.Diagnostics;
using RentBikeApi.Contracts;
using RentBikeApi.Domain.Common;
using Serilog;

namespace RentBikeApi.ErrorHandling;


public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IDiagnosticContext diagnosticContext) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ErrorResponse response;

        if (exception is DomainException domainException)
        {

            logger.LogWarning(exception, "Business rule enforced by the database: {Code}", domainException.Error.Code);
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            response = ErrorResponse.InvalidData(domainException.Error);
        }
        else
        {
            logger.LogError(exception, "Unhandled exception on {Method} {Path}", httpContext.Request.Method, httpContext.Request.Path);
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var error = ApiErrors.Internal(httpContext.TraceIdentifier);
            response = new ErrorResponse(ApiErrors.InternalErrorMessage, error.Code, error.Description);
        }

        diagnosticContext.Set("ErrorCode", response.Code);
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        return true; 
    }
}