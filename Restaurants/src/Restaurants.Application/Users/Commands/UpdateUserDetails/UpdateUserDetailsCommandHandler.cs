using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands.UpdateUserDetails;

internal class UpdateUserDetailsCommandHandler(
    ILogger<UpdateUserDetailsCommandHandler> logger,
    IUserContext userContext,
    IUserStore<User> userStore,
    IMapper mapper
) : IRequestHandler<UpdateUserDetailsCommand>
{
    public async Task Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
    {
        CurrentUser user = userContext.GetCurrentUser();

        logger.LogInformation("Updating user (id: {UserId}) with {@Request}", user.Id, request);

        var dbUser = await userStore.FindByIdAsync(user.Id, cancellationToken);

        if (dbUser is null)
        {
            throw new NotFoundException(nameof(User), user.Id);
        }

        // TODO check: if it fails, map this manually here.
        dbUser = mapper.Map(request, dbUser);

        await userStore.UpdateAsync(dbUser, cancellationToken);
    }
}
