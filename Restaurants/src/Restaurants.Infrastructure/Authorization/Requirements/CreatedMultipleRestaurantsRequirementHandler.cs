using Microsoft.AspNetCore.Authorization;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public class CreatedMultipleRestaurantsRequirementHandler(
    IRestaurantsRepository restaurantsRepository,
    IUserContext userContext
) : AuthorizationHandler<CreatedMultipleRestaurantsRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CreatedMultipleRestaurantsRequirement requirement
    )
    {
        var restaurants = await restaurantsRepository.GetAllAsync();
        var currentUser = userContext.GetCurrentUser();

        int userRestaurantsCreated = restaurants.Count(x => x.OwnerId == currentUser!.Id);

        if (userRestaurantsCreated >= requirement.MinimumRestaurantsCreated)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}
