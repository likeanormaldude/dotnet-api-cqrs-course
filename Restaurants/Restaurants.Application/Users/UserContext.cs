using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Restaurants.Application.Users;

public interface IUserContext
{
    CurrentUser GetCurrentUser();
}

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public CurrentUser GetCurrentUser()
    {
        var user = httpContextAccessor?.HttpContext?.User;

        if (user == null)
        {
            throw new InvalidOperationException("User context is not present");
        }

        bool isUserAuthenticated = user.Identity != null || (user.Identity?.IsAuthenticated ?? false);

        if (!isUserAuthenticated)
        {
            throw new InvalidOperationException("User is not authenticated");
        }

        string userId = user.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
        string email = user.FindFirst(x => x.Type == ClaimTypes.Email)!.Value;
        IEnumerable<string> roles = user.Claims.Where(x => x.Type == ClaimTypes.Role)!.Select(x => x.Value);

        return new CurrentUser(userId, email, roles);
    }
}
