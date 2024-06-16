using Microsoft.AspNetCore.Mvc;
using Restaurants.API.Controllers.Models;

namespace Restaurants.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastService _weatherForecastService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService weatherForecast)
        {
            _logger = logger;
            _weatherForecastService = weatherForecast;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        [HttpGet("simple")]
        public IEnumerable<WeatherForecast> GetSimple()
        {
            var result = _weatherForecastService.GetSimple();
            return result;
        }

        /**
         * .https://localhost:7255/api/WeatherForecast/generate/?numberOfResults=2
         */
        [HttpPost]
        [Route("generate")]
        public ObjectResult GetGeneratedResults([FromQuery] int numberOfResults, [FromBody] TemperatureRange temperatureRange)
        {
            if( numberOfResults <= 0)
            {
                return StatusCode(400, "\"numberOfResults\" needs to be >= 1");
            }

            if (temperatureRange.MaxTemperature <= temperatureRange.MinTemperature)
            {
                return StatusCode(400, "\"maxTempererature\" needs to be > than \"minTemperature\"");
            }

            var result = _weatherForecastService.Get(
                numberOfResults,
                temperatureRange.MinTemperature,
                temperatureRange.MaxTemperature
            );

            return Ok(result);
        }

        [HttpGet("currentDay")]
        public WeatherForecast GetCurrentDayForecast()
        {
            var result = _weatherForecastService.GetCurrentDay();
            return result;
        }

        [HttpPost]
        public string Hello([FromBody] string name)
        {
            return $"Hello {name}";
        }
    }
}
