using FluentValidation;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandValidator: BaseCommandValidator<UpdateRestaurantCommand>
{
    public UpdateRestaurantCommandValidator()
    {
        ApplyCommonRules();
    }
}
