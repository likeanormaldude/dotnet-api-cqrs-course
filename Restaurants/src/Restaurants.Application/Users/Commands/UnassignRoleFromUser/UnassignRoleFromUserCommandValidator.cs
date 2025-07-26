using FluentValidation;

namespace Restaurants.Application.Users.Commands.UnassignRoleFromUser;

public class UnassignRoleFromUserCommandValidator : AbstractValidator<UnassignRoleFromUserCommand>
{
    public UnassignRoleFromUserCommandValidator()
    {
        RuleFor(dto => dto.RoleName).NotEmpty().Length(3, 100);
        RuleFor(dto => dto.UserEmail).NotEmpty().Length(3, 100).EmailAddress();
    }
}
