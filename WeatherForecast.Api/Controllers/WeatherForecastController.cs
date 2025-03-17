using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Api.Model.Responses;
using WeatherForecast.Api.Repository;
using WeatherForecast.Api.Services;

namespace WeatherForecast.Api.Controllers;

[ApiController]
public class WeatherForecastController : ControllerBase
{
    private readonly ExternalWeatherForecastService _externalWeatherForecastService;
    private readonly GeoLocationRepository _repository;

    public WeatherForecastController(
        ExternalWeatherForecastService externalWeatherForecastService,
        GeoLocationRepository repository)
    {
        _externalWeatherForecastService = externalWeatherForecastService;
        _repository = repository;
    }

    [HttpGet("api/geolocation/{latitude}/{longitude}/weatherforecast")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OpenMeteoResponse?>> Get(
        double latitude, 
        double longitude, 
        CancellationToken cancellationToken)
    {
        var entity = await _repository.Get(latitude, longitude);

        if (entity == null) 
        {
            return NotFound();
        }

        return await _externalWeatherForecastService.Get(latitude, longitude, cancellationToken);
    }
}
