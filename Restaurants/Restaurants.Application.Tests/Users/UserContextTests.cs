using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Restaurants.Domain.Constants;
using Xunit;

namespace Restaurants.Application.Users.Tests;

public class UserContextTests
{
    [Fact()]
    public void GetCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUser()
    {
        string mockedId = "1";
        string mockedEmail = "test@test.com";
        string mockedNationality = "German";
        DateOnly mockedDateOfBirth = new DateOnly(1990, 1, 1);
        #region Arrange
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, mockedId),
            new(ClaimTypes.Email, mockedEmail),
            new(ClaimTypes.Role, UserRoles.Admin),
            new(ClaimTypes.Role, UserRoles.User),
            new("Nationality", mockedNationality),
            new("DateOfBirth", mockedDateOfBirth.ToString("yyyy-MM-dd")),
        };

        ClaimsPrincipal user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext { User = user });
        var userContext = new UserContext(httpContextAccessorMock.Object);
        #endregion


        #region Act
        var currentUser = userContext.GetCurrentUser();
        #endregion


        #region Assert
        currentUser.Should().NotBeNull();
        currentUser.Id.Should().Be(mockedId);
        currentUser.Email.Should().Be(mockedEmail);
        currentUser.Roles.Should().Contain(UserRoles.Admin, UserRoles.User);
        currentUser.Nationality.Should().Be(mockedNationality);
        currentUser.DateOfBirth.Should().Be(mockedDateOfBirth);
        #endregion
    }

    [Fact()]
    public void GetCurrentUser_WithUserContextNotPresent_ThrowsInvalidOperationException()
    {
        #region Arrange
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null);
        var userContext = new UserContext(httpContextAccessorMock.Object);
        #endregion


        #region Act
        Action act = () => userContext.GetCurrentUser();
        #endregion

        act.Should().Throw<InvalidOperationException>().WithMessage("User context is not present");
    }
}
