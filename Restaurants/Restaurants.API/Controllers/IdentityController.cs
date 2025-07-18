using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Users.Commands.AssignUserRole;
using Restaurants.Application.Users.Commands.UnassignRoleFromUser;
using Restaurants.Application.Users.Commands.UpdateUserDetails;
using Restaurants.Application.Users.Queries.GetUserRolesQuery;
using Restaurants.Domain.Constants;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/identity")]
[Authorize]
public class IdentityController(IMediator mediator) : ControllerBase
{
    [HttpPatch("user")]
    public async Task<IActionResult> UpdateUserDetails(UpdateUserDetailsCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPost("userRole")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> AssignUserRole(AssignUserRoleCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpGet("userRole/{username}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<ActionResult<IEnumerable<string>>> GetUserRoles([FromRoute] string username)
    {
        GetUserRolesQuery query = new(username);
        IEnumerable<string> userRoles = await mediator.Send(query);
        return Ok(userRoles);
    }

    [HttpDelete("userRole")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<ActionResult<IEnumerable<string>>> UnassignRoleFromUser(UnassignRoleFromUserCommand command)
    {
        bool isUnassigned = await mediator.Send(command);

        if (isUnassigned)
        {
            return NoContent();
        }

        return NotFound("User or role was not found.");
    }
}
