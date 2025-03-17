using System.Text.Json.Serialization;

namespace WeatherForecast.Api.Model.Responses;

public class OpenMeteoResponse
{
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("elevation")]
    public double Elevation { get; set; }

    [JsonPropertyName("generationtime_ms")]
    public double GenerationTimeMs { get; set; }

    [JsonPropertyName("utc_offset_seconds")]
    public int UtcOffsetSeconds { get; set; }

    [JsonPropertyName("timezone")]
    public string? Timezone { get; set; }

    [JsonPropertyName("timezone_abbreviation")]
    public string? TimezoneAbbrevation { get; set; }

    [JsonPropertyName("hourly")]
    public Hourly? Hourly { get; set; }

    [JsonPropertyName("hourly_units")]
    public HourlyUnits? HourlyUnits { get; set; }
}

public class Hourly
{
    [JsonPropertyName("time")]
    public DateTime[]? Time { get; set; }

    [JsonPropertyName("temperature_2m")]
    public double[]? Temperature2m { get; set; }

    [JsonPropertyName("relative_humidity_2m")]
    public int[]? RelativeHumidity2m { get; set; }

    [JsonPropertyName("precipitation")]
    public double[]? Precipitation { get; set; }
}

public class HourlyUnits
{
    [JsonPropertyName("time")]
    public string? Time { get; set; }

    [JsonPropertyName("temperature_2m")]
    public string? Temperature2m { get; set; }

    [JsonPropertyName("relative_humidity_2m")]
    public string? RelativeHumidity2m { get; set; }

    [JsonPropertyName("precipitation")]
    public string? Precipitation { get; set; }
}
