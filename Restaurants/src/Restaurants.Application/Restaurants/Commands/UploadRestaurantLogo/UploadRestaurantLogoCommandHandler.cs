using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UploadRestaurantLogo;

public class UploadRestaurantLogoCommandHandler(
    ILogger<UploadRestaurantLogoCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IRestaurantAuthorizationService restaurantAuthorizationService,
    IBlobStorageService blobStorageService
) : IRequestHandler<UploadRestaurantLogoCommand>
{
    public async Task Handle(UploadRestaurantLogoCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Uploading logo for restaurant with id: {request.RestaurantId}");

        Restaurant? restaurant =
            await restaurantsRepository.GetByIdAsync(request.RestaurantId)
            ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        bool isAuthorize = restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Update);

        if (!isAuthorize)
        {
            throw new ForbidException();
        }

        string logoUrl = await blobStorageService.UploadToBlobAsync(request.File, request.FileName);
        restaurant.LogoUrl = logoUrl;

        await restaurantsRepository.SaveChanges();
    }
}
