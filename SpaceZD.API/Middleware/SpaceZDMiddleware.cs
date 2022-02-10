using Microsoft.Data.SqlClient;
using SpaceZD.BusinessLayer.Exceptions;
using System.Net;
using System.Text.Json;

namespace SpaceZD.API.Middleware;

public class SpaceZdMiddleware
{
    private readonly RequestDelegate _next;

    public SpaceZdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AuthorizationException ex)
        {
            await HandleExceptionAsync(context, (HttpStatusCode)403, ex.Message);
        }
        catch (NotFoundException ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (SqlException ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.ServiceUnavailable, "БД не алё");
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, HttpStatusCode code, string message)
    {
        var result = JsonSerializer.Serialize(new { error = message });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        await context.Response.WriteAsync(result);
    }
}