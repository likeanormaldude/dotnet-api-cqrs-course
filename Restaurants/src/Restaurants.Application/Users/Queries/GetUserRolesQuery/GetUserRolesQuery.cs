using MediatR;

namespace Restaurants.Application.Users.Queries.GetUserRolesQuery;

public class GetUserRolesQuery(string username) : IRequest<IEnumerable<string>>
{
    public string Username { get; set; } = username;
}
