using Microsoft.Extensions.Caching.Hybrid;
using WeatherForecast.Api.Configs;
using WeatherForecast.Api.Exceptions;
using WeatherForecast.Api.Model.Responses;

namespace WeatherForecast.Api.Services;

public class ExternalWeatherForecastService
{
    private readonly HttpClient _client;
    private readonly HybridCache _cache;
    private readonly OpenMeteoOptions? _openMeteoOptions;

    public ExternalWeatherForecastService(
        IConfiguration configuration,
        HttpClient client,
        HybridCache cache)
    {
        _client = client;
        _cache = cache;
        _openMeteoOptions = configuration
            .GetSection(OpenMeteoOptions.OpenMeteo)
            .Get<OpenMeteoOptions>();
    }

    public async Task<OpenMeteoResponse?> Get(
        double latitude, 
        double longitude,
        CancellationToken token)
    {
        var entryOptions = new HybridCacheEntryOptions
        {
            Expiration = _openMeteoOptions?.RefreshTimeSpan
        };

        if (token.IsCancellationRequested) { 
            return null;
        }

        return await _cache.GetOrCreateAsync<OpenMeteoResponse?>(
            $"{latitude}/{longitude}",
            async cancel => await GetExternalServiceData(latitude, longitude, cancel),
            entryOptions,
            cancellationToken: token);
    }

    private async Task<OpenMeteoResponse?> GetExternalServiceData(
        double latitude,
        double longitude,
        CancellationToken token)
    {
        var parameters = new Dictionary<string, string>
        {
            { "latitude", latitude.ToString() },
            { "longitude", longitude.ToString() },
            { "hourly", "temperature_2m,relative_humidity_2m,precipitation" },
        };

        var content = new FormUrlEncodedContent(parameters);

        if (token.IsCancellationRequested)
        {
            return null;
        }

        var response = await _client.PostAsync(_openMeteoOptions?.Url, content, token);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<OpenMeteoResponse>(token);
        }

        throw new ExternalWeatherForecastException(
            $"Cannot get open meteo weather forecast. StatusCode: {response.StatusCode}.");
    }
}
