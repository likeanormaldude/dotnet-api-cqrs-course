using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    public CreateRestaurantCommandValidator()
    {
        RuleFor(dto => dto.Name).NotEmpty().Length(3, 100);

        RuleFor(dto => dto.Description).NotEmpty().WithMessage("Description is required.");

        RuleFor(dto => dto.Category).NotEmpty().WithMessage("Category is required.");

        RuleFor(dto => dto.ContactEmail)
            .EmailAddress()
            .When(dto => !string.IsNullOrEmpty(dto.ContactEmail))
            .WithMessage("Please provide a valid email address");

        RuleFor(dto => dto.PostalCode)
            .Matches(@"^((\S{3})(\s{1})?(\S{3}))$")
            .WithMessage("Please provide a valid postal code in the format \"XXX XXX\"");
    }
}
