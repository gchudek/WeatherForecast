using System.ComponentModel.DataAnnotations;

namespace WeatherForecast.Api.Model.Requests;

public class GeoLocationRequest
{
    [Range(-90d, 90.0d)]
    [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Two digits precision allowed")]
    public double Latitude { get; set; }

    [Range(-180d, 180d)]
    [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Two digits precision allowed")]
    public double Longitude { get; set; }
}
