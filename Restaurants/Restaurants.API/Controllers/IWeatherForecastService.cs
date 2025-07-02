namespace Restaurants.API.Controllers;

public interface IWeatherForecastService
{
    IEnumerable<WeatherForecast> Get(
        int take = 5,
        int minimumTemperature = 0,
        int maximumTemperature = 0
    );
}
