using MediatR;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Users.Queries.GetUserDetailsQuery;

public class GetUserDetailsQuery(string username) : IRequest<User?>
{
    public string Username { get; set; } = username;
}
