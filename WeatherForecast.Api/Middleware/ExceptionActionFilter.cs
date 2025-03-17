using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Data;
using WeatherForecast.Api.Exceptions;

namespace WeatherForecast.Api.Middleware;

public class ExceptionActionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case ExternalWeatherForecastException ex:
                context.Result = CreateResult(
                    StatusCodes.Status503ServiceUnavailable, 
                    context.Exception.Message);
                break;
            case ConstraintException ex:
                context.Result = CreateResult(
                    StatusCodes.Status409Conflict,
                    context.Exception.Message);
                break;
            default:
                context.Result = CreateResult(
                    StatusCodes.Status500InternalServerError,
                    context.Exception.Message);
                break;
        }
    }

    private static IActionResult CreateResult(int statusCode, string message)
    {
        return new ContentResult()
        {
            StatusCode = statusCode,
            Content = message
        };
    }
}
