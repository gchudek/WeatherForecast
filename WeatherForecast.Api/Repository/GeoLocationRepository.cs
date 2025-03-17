using Microsoft.EntityFrameworkCore;
using System.Data;
using WeatherForecast.Api.Model;

namespace WeatherForecast.Api.Repository;

public class GeoLocationRepository
{
    private readonly WeatherForecastContext _context;

    public GeoLocationRepository(WeatherForecastContext context) 
    { 
        _context = context;
    }

    public async Task<IEnumerable<GeoLocation>> GetAll()
    {
        return await _context
            .GeoLocations
            .ToListAsync();
    }

    public async Task<GeoLocation?> Get(int id)
    {
        var entity = await _context
            .GeoLocations
            .FindAsync(id);

        return entity;
    }

    public async Task<GeoLocation?> Get(double latitude, double longitude)
    {
        var entity = await _context
            .GeoLocations
            .Where(p =>
                    p.Latitude == latitude &&
                    p.Longitude == longitude)
            .FirstOrDefaultAsync();

        return entity;
    }

    public async Task<int> Add(GeoLocation entity)
    {
        // Checking unique should be done on database level
        // This is only workaround for in memory database
        await CheckGeoLocationUniqueConstraint(entity);
        
        _context.Add(entity);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> Delete(GeoLocation entity)
    {
        _context.Remove(entity);
        return await _context.SaveChangesAsync();
    }

    private async Task CheckGeoLocationUniqueConstraint(GeoLocation entity)
    {
        var existingEntity = await Get(entity.Latitude, entity.Longitude);
        if (existingEntity != null)
        {
            throw new ConstraintException("Entity already exists.");
        }
    }
}
