using FluentAssertions;
using Restaurants.Domain.Constants;
using Xunit;

namespace Restaurants.Application.Users.Tests;

public class CurrentUserTests
{
    // Suggested naming convention for test methods: // [MethodName]_[Condition]_[ExpectedResult]
    // Suggested steps for writing tests: Arrange, Act, Assert

    [Theory()]
    [InlineData(UserRoles.Admin)]
    [InlineData("admin")]
    [InlineData("ADMIN")]
    [InlineData(UserRoles.User)]
    public void IsInRole_WithMatchingRole_ShouldReturnTrue(string roleName)
    {
        // Arrange
        var currentUser = new CurrentUser("1", "mail@test.com", [UserRoles.Admin, UserRoles.User], null, null);

        // Act
        bool isInRole = currentUser.IsInRole(roleName);

        // Assert
        isInRole.Should().BeTrue();
    }

    [Fact()]
    public void IsInRole_WithNoMatchingRole_ShouldReturnFalse()
    {
        // Arrange
        var currentUser = new CurrentUser("1", "mail@test.com", [UserRoles.Admin, UserRoles.User], null, null);

        // Act
        bool isInRole = currentUser.IsInRole(UserRoles.Owner);

        // Assert
        isInRole.Should().BeFalse();
    }

    [Fact()]
    public void IsInRole_WithNoMatchingRoleCase_ShouldReturnFalse()
    {
        string dummyRoleShouldBeIgnored = "dummyRole";
        // Arrange
        var currentUser = new CurrentUser("1", "mail@test.com", [UserRoles.Admin, UserRoles.User], null, null);

        // Act
        bool isInRole = currentUser.IsInRole(dummyRoleShouldBeIgnored);

        // Assert
        isInRole.Should().BeFalse();
    }
}
