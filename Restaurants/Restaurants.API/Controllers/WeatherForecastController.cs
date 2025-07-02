using Microsoft.AspNetCore.Mvc;
using Restaurants.API.Controllers;

namespace Restaurants.Controllers;

public class TemperatureRequest
{
    public int Min { get; set; }
    public int Max { get; set; }
}

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IWeatherForecastService _weatherForecastService;
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(
        ILogger<WeatherForecastController> logger,
        IWeatherForecastService weatherForecastService
    )
    {
        _logger = logger;
        _weatherForecastService = weatherForecastService;
    }

    [HttpPost("generate")]
    // /generate?count=4
    public IActionResult Generate([FromQuery] int count, [FromBody] TemperatureRequest request)
    {
        if (count < 0 || request.Max < request.Min)
        {
            return BadRequest(
                "Count has to be positive number, and max must be greather than the min value"
            );
        }

        IEnumerable<WeatherForecast> result = _weatherForecastService.Get(
            count,
            request.Min,
            request.Max
        );
        return Ok(result);
    }

    [HttpGet]
    [Route("currentDay")]
    public WeatherForecast GetCurrentDayForecast()
    {
        var result = _weatherForecastService.Get().First();
        return result;
    }
}
