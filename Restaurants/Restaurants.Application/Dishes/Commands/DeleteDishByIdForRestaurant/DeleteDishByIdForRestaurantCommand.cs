using MediatR;

namespace Restaurants.Application.Dishes.Commands.DeleteDishByIdForRestaurant;

public class DeleteDishByIdForRestaurantCommand(int restaurantId, int dishId) : IRequest<bool>
{
    public int RestaurantId { get; set; } = restaurantId;
    public int DishId { get; set; } = dishId;
}
