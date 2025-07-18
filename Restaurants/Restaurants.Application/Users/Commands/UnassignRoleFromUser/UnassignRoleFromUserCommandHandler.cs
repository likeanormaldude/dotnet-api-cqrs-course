using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users.Commands.AssignUserRole;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands.UnassignRoleFromUser;

public class UnassignRoleFromUserCommandHandler(
    ILogger<AssignUserRoleCommandHandler> logger,
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager
) : IRequestHandler<UnassignRoleFromUserCommand, bool>
{
    public async Task<bool> Handle(UnassignRoleFromUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Unassigning role: {Role} from user {User}", request.RoleName, request.UserEmail);

        var user =
            await userManager.FindByEmailAsync(request.UserEmail)
            ?? throw new NotFoundException(nameof(User), request.UserEmail);

        var role =
            await roleManager.FindByNameAsync(request.RoleName)
            ?? throw new NotFoundException(nameof(IdentityRole), request.RoleName);

        await userManager.RemoveFromRoleAsync(user, role.Name!);

        return true;
    }
}
