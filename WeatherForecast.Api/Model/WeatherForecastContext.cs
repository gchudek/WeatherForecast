using Microsoft.EntityFrameworkCore;

namespace WeatherForecast.Api.Model;

public class WeatherForecastContext : DbContext
{
    public WeatherForecastContext(DbContextOptions<WeatherForecastContext> options)
        : base(options)
    {
    }

    public DbSet<GeoLocation> GeoLocations { get; set; } = null!;
}
