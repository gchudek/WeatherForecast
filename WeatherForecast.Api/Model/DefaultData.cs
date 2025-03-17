namespace WeatherForecast.Api.Model;

public static class DefaultData
{
    private static object LockObject = new object();

    public static void Load(WeatherForecastContext context)
    {
        lock (LockObject)
        { 
            if (!context.GeoLocations.Any())
            {
                context.GeoLocations.AddRange(GeoLocations);
            }

            context.SaveChanges();
        }
    }

    private static GeoLocation[] GeoLocations
    {
        get
        {
            return [
                new GeoLocation() { Latitude = 50.90, Longitude = 16.74 },
                new GeoLocation() { Latitude = 51.11, Longitude = 17.03 },
                new GeoLocation() { Latitude = 52.40, Longitude = 16.93 },
                new GeoLocation() { Latitude = 50.06, Longitude = 19.94 },
                new GeoLocation() { Latitude = 52.23, Longitude = 21.01 },
                new GeoLocation() { Latitude = 54.35, Longitude = 18.65 },
                new GeoLocation() { Latitude = 51.76, Longitude = 19.46 }
            ];
        }
    }
}
