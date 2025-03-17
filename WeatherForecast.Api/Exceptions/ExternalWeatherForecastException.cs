namespace WeatherForecast.Api.Exceptions;

public class ExternalWeatherForecastException : Exception
{
    public ExternalWeatherForecastException()
    {
    }

    public ExternalWeatherForecastException(string message)
        : base(message)
    {
    }

    public ExternalWeatherForecastException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
