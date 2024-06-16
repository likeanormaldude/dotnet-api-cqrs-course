
namespace Restaurants.API.Controllers
{
    public interface IWeatherForecastService
    {
        IEnumerable<WeatherForecast> GetSimple();
        IEnumerable<WeatherForecast> Get(int numberOfResults, int minTemperature, int maxTempererature);
        WeatherForecast GetCurrentDay();
    }
}