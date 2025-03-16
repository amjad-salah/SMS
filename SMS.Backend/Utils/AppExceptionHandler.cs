using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using SMS.Models.DTOs;

namespace SMS.Backend.Utils;

public class AppExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
        Exception exception, CancellationToken cancellationToken)
    {
        Log.Error("{StackTree}- {Instance}- {Message}", exception.StackTrace,
            exception.InnerException, exception.Message);

        var response = new BaseResponseDto()
        {
            Success = false,
            Message = "Internal Server Error, pls try again later"
        };

        httpContext.Response.StatusCode = 500;
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}