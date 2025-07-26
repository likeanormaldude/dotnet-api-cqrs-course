using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Infrastructure.Authorization.Services;

public class RestaurantAuthorizationService(ILogger<RestaurantAuthorizationService> logger, IUserContext userContext)
    : IRestaurantAuthorizationService
{
    public bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation)
    {
        var user = userContext.GetCurrentUser();

        logger.LogInformation(
            "Authorizing user {UserEmail}, to {Operation} for restaurant {RestaurantName}",
            user.Email,
            resourceOperation,
            restaurant.Name
        );

        bool isCanReadOrCreate =
            resourceOperation == ResourceOperation.Read || resourceOperation == ResourceOperation.Create;

        if (isCanReadOrCreate)
        {
            logger.LogInformation("Create/Read operation - successful authorization");
            return true;
        }

        bool isCanDelete = resourceOperation == ResourceOperation.Delete && user.IsInRole(UserRoles.Admin);

        if (isCanDelete)
        {
            logger.LogInformation("Admin user, delete operation - successful authorization");
            return true;
        }

        bool isCanUpdate = resourceOperation == ResourceOperation.Update && user.Id == restaurant.OwnerId;

        if (isCanUpdate)
        {
            logger.LogInformation("Restaurant owner - successful authorization");
            return true;
        }

        return false;
    }
}
