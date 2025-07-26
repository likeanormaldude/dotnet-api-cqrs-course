using MediatR;
using Microsoft.AspNetCore.Identity;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Queries.GetUserRolesQuery;

public class GetUserRolesQueryHandler(UserManager<User> userManager)
    : IRequestHandler<GetUserRolesQuery, IEnumerable<string>>
{
    public async Task<IEnumerable<string>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        var user =
            await userManager.FindByNameAsync(request.Username)
            ?? throw new NotFoundException(nameof(User), request.Username);

        IEnumerable<string> roles = await userManager.GetRolesAsync(user!);
        return roles;
    }
}
