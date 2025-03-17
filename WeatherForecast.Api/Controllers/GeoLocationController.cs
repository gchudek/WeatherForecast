using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Api.Model;
using WeatherForecast.Api.Model.Requests;
using WeatherForecast.Api.Model.Responses;
using WeatherForecast.Api.Repository;

namespace WeatherForecast.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GeoLocationController : ControllerBase
{
    private readonly GeoLocationRepository _repository;

    public GeoLocationController(GeoLocationRepository repository) 
    { 
        _repository = repository;
    }  

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GeoLocationResponse>>> Get()
    {
        var all = await _repository.GetAll();

        return all
            .Select(EntityToResponse)
            .ToList();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GeoLocationResponse>> Get(int id)
    {
        var item = await _repository.Get(id);

        if (item == null)
        {
            return NotFound();
        }

        return EntityToResponse(item);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<GeoLocationResponse>> Post([FromBody]GeoLocationRequest item)
    {
        var entity = new GeoLocation()
        {
            Latitude = item.Latitude,
            Longitude = item.Longitude
        };

        await _repository.Add(entity);

        return CreatedAtAction(
            nameof(Get),
            new { id = entity.Id }, 
            EntityToResponse(entity));
    }

    [HttpDelete("{latitude}/{longitude}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(double latitude, double longitude)
    {
        var entity = await _repository.Get(latitude, longitude);

        if (entity == null)
        {
            return NotFound();
        }

        await _repository.Delete(entity);
        
        return NoContent();
    }

    private static GeoLocationResponse EntityToResponse(GeoLocation entity) =>
       new GeoLocationResponse
       {
           Id = entity.Id,
           Latitude = entity.Latitude, 
           Longitude = entity.Longitude,
       };
}
