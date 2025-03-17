using System.Net;
using System.Net.Http.Json;
using WeatherForecast.Api.Model.Requests;

namespace WeatherForecast.IntegrationTests.Cotrollers;

public class GeoLocationControllerTest : IClassFixture<WebAppFactory>
{
    private HttpClient _httpClient;

    public GeoLocationControllerTest(WebAppFactory factory) 
    {
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        var result = await _httpClient.GetAsync("/api/geolocation");

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async Task Post_NewLocation_ReturnsCreated()
    {
        var value = new GeoLocationRequest()
        {
            Latitude = 51.25,
            Longitude = 22.57
        };

        var result = await _httpClient.PostAsJsonAsync<GeoLocationRequest>("/api/geolocation", value);

        Assert.Equal(HttpStatusCode.Created, result.StatusCode);
        Assert.True(result.Headers.Contains("Location"));
    }

    [Fact]
    public async Task Post_IncorrectLatitude_ReturnsBadRequest()
    {
        var value = new GeoLocationRequest()
        {
            Latitude = 151.25,
            Longitude = 22.57
        };

        var result = await _httpClient.PostAsJsonAsync<GeoLocationRequest>("/api/geolocation", value);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
    }


    [Fact]
    public async Task Post_IncorrectLatitudeCornerCase_ReturnsBadRequest()
    {
        var value = new GeoLocationRequest()
        {
            Latitude = -90.1,
            Longitude = 22.57
        };

        var result = await _httpClient.PostAsJsonAsync<GeoLocationRequest>("/api/geolocation", value);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
    }

    [Fact]
    public async Task Post_IncorrectLongitude_ReturnsBadRequest()
    {
        var value = new GeoLocationRequest()
        {
            Latitude = 51.25,
            Longitude = 222.57
        };

        var result = await _httpClient.PostAsJsonAsync<GeoLocationRequest>("/api/geolocation", value);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
    }

    [Fact]
    public async Task Post_TwoTheSameLocations_ReturnsConflict()
    {
        var value = new GeoLocationRequest()
        {
            Latitude = 50.87,
            Longitude = 20.63
        };

        var result = await _httpClient.PostAsJsonAsync<GeoLocationRequest>("/api/geolocation", value);
        
        result = await _httpClient.PostAsJsonAsync<GeoLocationRequest>("/api/geolocation", value);

        Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);
    }

    [Fact]
    public async Task GetById_ExistingData_ReturnsOk()
    {
        var value = new GeoLocationRequest()
        {
            Latitude = 53.12,
            Longitude = 18.01
        };

        var result = await _httpClient.PostAsJsonAsync<GeoLocationRequest>("/api/geolocation", value);
        string? location = result.Headers.GetValues("Location").FirstOrDefault();

        result = await _httpClient.GetAsync(location);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async Task GetById_NoData_ReturnsNotFound()
    {
        var result = await _httpClient.GetAsync("/api/geolocation/222");

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }

    [Fact]
    public async Task Delete_FirstOutOfDefaultData_ReturnsNoContent()
    {
        var value = new GeoLocationRequest()
        {
            Latitude = 53.01,
            Longitude = 18.60
        };

        var result = await _httpClient.PostAsJsonAsync<GeoLocationRequest>("/api/geolocation", value);

        result = await _httpClient.DeleteAsync($"/api/geolocation/{value.Latitude}/{value.Longitude}");

        Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
    }

    [Fact]
    public async Task Delete_NoData_ReturnsNotFound()
    {
        var result = await _httpClient.DeleteAsync("/api/geolocation/51.9/16.74");

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }
}
