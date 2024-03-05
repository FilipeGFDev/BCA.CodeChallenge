namespace Car.Auction.Management.System.Web.Middlewares;

using FluentValidation;
using Car.Auction.Management.System.Models.ErrorCodes;
using Car.Auction.Management.System.Models.Exceptions;
using Car.Auction.Management.System.Models.Exceptions.Vehicle;
using global::System.Net;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (ValidationException ex)
        {
            await SetExceptionResponse(
                httpContext,
                HttpStatusCode.BadRequest,
                ex.Errors.Select(x => new { x.ErrorCode, x.ErrorMessage }));
        }
        catch (NotFoundException ex)
        {
            await SetExceptionResponse(
                httpContext,
                HttpStatusCode.NotFound,
                new { ex.Message });
        }
        catch (InvalidVehicleTypeException ex)
        {
            await SetExceptionResponse(
                httpContext,
                HttpStatusCode.BadRequest,
                new { ex.Message });
        }
        catch (Exception _)
        {
            await SetExceptionResponse(
                httpContext,
                HttpStatusCode.InternalServerError,
                new ErrorCode(
                    ErrorCodes.Application.UnexpectedError.Code,
                    ErrorCodes.Application.UnexpectedError.ErrorMessage));
        }
    }

    private static async Task SetExceptionResponse<T>(
        HttpContext httpContext,
        HttpStatusCode statusCode,
        T? message)
    {
        httpContext.Response.StatusCode = (int)statusCode;
        await httpContext.Response.WriteAsJsonAsync(message);
    }
}

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        => builder.UseMiddleware<ExceptionMiddleware>();
}