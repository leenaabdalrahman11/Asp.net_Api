using System;
using Microsoft.AspNetCore.Diagnostics;
using MyApiProject.MyApi.DAL.DTO.Response;

namespace MyApiProject.MyApi.PLL;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        var errorDetails = new ErrorDetails
            {
                Message = "server error ...",
             //   StackTrace = ex.InnerException?.Message ?? ex.Message,
                StatusCode = StatusCodes.Status500InternalServerError
            };
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(errorDetails);
        return true;

    }
}