using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using MusicApi;
using MusicApi.Data;
using OpenTelemetry.Trace;

// ReSharper disable ExplicitCallerInfoArgument

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.AddServiceDefaults();
}
else
{
    // builder.Logging.ClearProviders();
    // Console.WriteLine("Console logging disabled");
    builder.ConfigureOpenTelemetry();
}

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing.AddSource("MusicApi");
        tracing.SetSampler(new TraceIdRatioBasedSampler(0.1));
    });

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, new AppJsonSerializerContext());
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContextPool<MusicDbContext>(builder =>
{
    builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", (ILoggerFactory loggerFactory) =>
    {
        var logger = loggerFactory.CreateLogger("WeatherForecast");
        logger.LogInformation("Getting weather forecast");
        try
        {
            // var millisecondsDelay = Random.Shared.Next(10, 200);
            // using (var activity = Telemetry.ActivitySource.StartActivity("FakeDelay", ActivityKind.Client))
            // {
            //     activity?.AddTag("delay", millisecondsDelay);
            //     await Task.Delay(millisecondsDelay);
            // }

            if (Random.Shared.Next(100) == 50)
            {
                throw new Exception("Fake exception");
            }

            var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    (
                        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        Random.Shared.Next(-20, 55),
                        summaries[Random.Shared.Next(summaries.Length)]
                    ))
                .ToArray();
            logger.LogInformation("Weather forecast returned");
            Activity.Current?.AddEvent(new ActivityEvent("Weather forecast returned", tags: new ActivityTagsCollection
            {
                ["forecast.count"] = forecast.Length
            }));
            return forecast;
        }
        catch (Exception exception)
        {
            var activity = Activity.Current;
            if (activity != null)
            {
                activity.AddException(exception);
                activity.SetStatus(Status.Error);
            }
            
            logger.LogError(exception, "Error getting weather forecast");
            throw;
        }
    })
    .WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

[JsonSerializable(typeof(WeatherForecast[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}
