using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;

namespace Restaurants.API.Controllers
{

    [ApiController]
    [Route("api/restaurants")]
    public class RestaurantsController
    (
        IRestaurantsService restaurantsService
    ): ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult>  GetAll()
        {
            var restaurants = await restaurantsService.GetAllRestaurants();
            return Ok(restaurants);
        }

        // GET - .https://localhost:7255/api/restaurants/12
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRestaurant([FromRoute] int id)
        {
            var restaurant = await restaurantsService.GetById(id);

            if(restaurant == null)
            {
                return NotFound();
            }

            return Ok(restaurant);
        }
    }
}