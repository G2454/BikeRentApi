using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RentBikeApi.Contracts;
using Serilog;

namespace RentBikeApi.ErrorHandling;
public sealed class ErrorCodeLoggingFilter(IDiagnosticContext diagnosticContext) : IAlwaysRunResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is ObjectResult { Value: ErrorResponse error })
            diagnosticContext.Set("ErrorCode", error.Code);
    }

    public void OnResultExecuted(ResultExecutedContext context) { }
}