using MediatR;

namespace Restaurants.Application.Restaurants.Commands.UploadRestaurantLogo;

public class UploadRestaurantLogoCommand(int restaurantId) : IRequest
{
    public int RestaurantId { get; set; } = restaurantId;
    public string FileName { get; set; } = default!;
    public Stream File { get; set; } = default!;
}
