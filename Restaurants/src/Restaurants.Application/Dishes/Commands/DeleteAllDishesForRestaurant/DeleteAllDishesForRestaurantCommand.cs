using MediatR;

namespace Restaurants.Application.Dishes.Commands.DeleteAllDishesForRestaurant;

public class DeleteAllDishesForRestaurantCommand(int restaurantId) : IRequest<bool>
{
    public int RestaurantId { get; set; } = restaurantId;
}
