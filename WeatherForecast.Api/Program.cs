using Microsoft.EntityFrameworkCore;
using WeatherForecast.Api.Middleware;
using WeatherForecast.Api.Model;
using WeatherForecast.Api.Repository;
using WeatherForecast.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionActionFilter>(); 
});

builder.Services.AddOpenApi();
builder.Services.AddHybridCache();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<WeatherForecastContext>(opt =>
    opt.UseInMemoryDatabase("WeatherForecastList"));

builder.Services.AddScoped<GeoLocationRepository>();
builder.Services.AddScoped<ExternalWeatherForecastService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();

    //// Workaround to load default data.
    //// Data seeding does not work with in memory database. 
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<WeatherForecastContext>();
        DefaultData.Load(context!);
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
