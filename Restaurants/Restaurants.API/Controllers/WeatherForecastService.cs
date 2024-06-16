using Microsoft.AspNetCore.Mvc;
using System;

namespace Restaurants.API.Controllers
{
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
            "Scorching"
        };

        public IEnumerable<WeatherForecast> GetSimple()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        public IEnumerable<WeatherForecast> Get(
            [FromQuery] int numberOfResults,
            [FromQuery] int minTemperature,
            [FromQuery] int maxTempererature
        )
        {
            return Enumerable.Range(1, numberOfResults)
                .Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(minTemperature, maxTempererature),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();
        }

        public WeatherForecast GetCurrentDay()
        {
            var result = new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            };

            return result;
        }

    }
}
