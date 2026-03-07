using System;
using MyApi.DAL.Models;
using MyApiProject.MyApi.DAL.DTO.Response;

namespace MyApiProject.MyApi.PLL.Middleware;

public class GlobalExceptionHandling
{
    private readonly RequestDelegate _next;
    public GlobalExceptionHandling(RequestDelegate next)
    {
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
            var errorDetails = new ErrorDetails
            {
                Message = "server error ...",
                StackTrace = ex.InnerException?.Message ?? ex.Message,
                StatusCode = StatusCodes.Status500InternalServerError
            };
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(errorDetails);
        }
    }

}
