using FluentValidation;
using Restaurants.Application.Dtos;

namespace Restaurants.Application.Restaurants.Validators;

public class CreateRestaurantvalidator: AbstractValidator<CreateRestaurantDto>
{

    private readonly List<string> validCategories = ["Italian", "Brazilian", "Japenese", "Chinese", "American", "Vietnamese", "Korean"];

    public CreateRestaurantvalidator()
    {
        RuleFor(dto => dto.Name)
            .Length(3, 100);

        RuleFor(dto => dto.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(dto => dto.Category)
            .Custom((value, context) =>
            {
                var isValidCategory = validCategories.Contains(value);

                if (!isValidCategory)
                    context.AddFailure("Category", "Invalid Category. Please choose from the valid categories.");
            });

        RuleFor(dto => dto.ContactEmail)
            .EmailAddress().WithMessage("Please provide a valid e-mail address");

        RuleFor(dto => dto.PostalCode)
            .Matches(@"^(\S{3})(\s)?(\S{3})$").WithMessage("Please provide a valid post code (XXX XXX).");

    }
}
