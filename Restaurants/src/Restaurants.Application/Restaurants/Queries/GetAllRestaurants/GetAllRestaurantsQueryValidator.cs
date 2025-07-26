using FluentValidation;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{
    private int[] allowedPageSizes { get; set; } = [5, 10, 15, 30];
    private string[] allowedSortByColumnNames { get; set; } =
        [nameof(RestaurantDto.Name), nameof(RestaurantDto.Description), nameof(RestaurantDto.Category)];

    public GetAllRestaurantsQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .Must(value => allowedPageSizes.Contains(value))
            .WithMessage($"Page size must be in [{string.Join(",", allowedPageSizes)}]");

        RuleFor(x => x.SortBy)
            .Must(value => allowedSortByColumnNames.Contains(value))
            .When(x => x.SortBy is not null)
            .WithMessage($"Sort by is optional, or must be in [{string.Join(",", allowedSortByColumnNames)}]");
    }
}
