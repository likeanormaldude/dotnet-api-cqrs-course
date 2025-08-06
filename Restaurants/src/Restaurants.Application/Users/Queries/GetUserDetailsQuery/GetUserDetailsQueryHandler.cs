using MediatR;
using Microsoft.AspNetCore.Identity;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Users.Queries.GetUserDetailsQuery;

public class GetUserDetailsQueryHandler(UserManager<User> userManager) : IRequestHandler<GetUserDetailsQuery, User?>
{
    public async Task<User?> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(request.Username);
        return user;
    }
}
