namespace WeatherForecast.Api.Configs;

public class OpenMeteoOptions
{
    public const string OpenMeteo = "OpenMeteo";

    public string Url { get; set; } = string.Empty;
    public TimeSpan? RefreshTimeSpan { get; set; } = null;
}
