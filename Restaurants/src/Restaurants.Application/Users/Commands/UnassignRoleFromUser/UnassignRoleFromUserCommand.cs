using MediatR;

namespace Restaurants.Application.Users.Commands.UnassignRoleFromUser;

public class UnassignRoleFromUserCommand(string roleName, string userEmail) : IRequest<bool>
{
    public string RoleName { get; set; } = roleName;
    public string UserEmail { get; set; } = userEmail;
}
