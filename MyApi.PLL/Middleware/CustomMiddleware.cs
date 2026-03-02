using System;

namespace MyApiProject.MyApi.PLL.Middleware;

public class CustomMiddleware
{
    private readonly RequestDelegate _next;
    public CustomMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        // Custom logic before the next middleware
        Console.WriteLine("Custom Middleware: Before next middleware");

        await _next(context);

        // Custom logic after the next middleware
        Console.WriteLine("Custom Middleware: After next middleware");
    }

}
