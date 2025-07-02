namespace Restaurants.API.Controllers;

public class WeatherForecastService : IWeatherForecastService
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing",
        "Bracing",
        "Chilly",
        "Cool",
        "Mild",
        "Warm",
        "Balmy",
        "Hot",
        "Sweltering",
    };

    public IEnumerable<WeatherForecast> Get(
        int take,
        int minimumTemperature,
        int maximumTemperature
    )
    {
        IEnumerable<WeatherForecast> forecasts = Enumerable
            .Range(1, take)
            .Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(minimumTemperature, maximumTemperature),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
            });

        return forecasts;
    }
}
