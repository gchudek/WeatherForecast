# WeatherForecast

This is a simple REST API to store a weather forecast geo locations and provide the weather forecast delivered by 3rd party API.

The API has been written in `ASP.NET Core 9.0`.

Precision of coordinates has been set up to 2 digits (limitation of 3rd party API)

## Endpoints

The documentation is available on Swagger UI - link: `http://localhost:5000/swagger/index.html`

## Starting the project

### Build docker image
    docker build -f WeatherForecast.Api\Dockerfile -t weather-forecast .
### Run image
    docker run --rm -d -e ASPNETCORE_ENVIRONMENT=Development -p 5000:8080 weather-forecast

## Integration tests
### Run integration tests
    dotnet test .\WeatherForecast.IntegrationTests\WeatherForecast.IntegrationTests.csproj
