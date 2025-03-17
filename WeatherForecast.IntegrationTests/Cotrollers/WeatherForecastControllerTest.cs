using System.Net;
using System.Net.Http.Json;
using WeatherForecast.Api.Model.Requests;

namespace WeatherForecast.IntegrationTests.Cotrollers;

public class WeatherForecastControllerTest : IClassFixture<WebAppFactory>
{
    private readonly HttpClient _httpClient;    

    public WeatherForecastControllerTest(WebAppFactory factory)
    {
        _httpClient = factory.CreateClient();
    }

    [Fact]        
    public async Task Get_ExistingCoordinates_ReturnsWeatherForecast()
    {
        var value = new GeoLocationRequest()
        {
            Latitude = 50.15,
            Longitude = 19.02
        };

        var result = await _httpClient.PostAsJsonAsync<GeoLocationRequest>("/api/geolocation", value);

        result = await _httpClient.GetAsync($"/api/geolocation/{value.Latitude}/{value.Longitude}/weatherforecast");

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async Task Get_MissingCoordinates_ReturnsNotFound()
    {
        var result = await _httpClient.GetAsync("/api/geolocation/50.91/16.74/weatherforecast");

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }

    [Fact]
    public async Task Get_IncorrectCoordinates_ReturnsNotFound()
    {
        var result = await _httpClient.GetAsync("/api/geolocation/150.91/16.74/weatherforecast");

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }
}