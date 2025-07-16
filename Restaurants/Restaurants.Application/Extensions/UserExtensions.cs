using System.Security.Claims;

namespace Restaurants.Application.Extensions;

public static class UserExtensions
{
    public static IEnumerable<string> GetRoles(this ClaimsPrincipal user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User cannot be null");
        }

        IEnumerable<string> roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

        return roles.Any() ? roles : new List<string>();
    }
}
