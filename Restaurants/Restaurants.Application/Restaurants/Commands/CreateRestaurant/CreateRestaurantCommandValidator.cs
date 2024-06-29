namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidator : BaseCommandValidator<CreateRestaurantCommand>
{
    public CreateRestaurantCommandValidator()
    {
        ApplyCommonRules();
    }
}
