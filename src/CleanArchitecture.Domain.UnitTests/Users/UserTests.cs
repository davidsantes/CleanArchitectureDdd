using CleanArchitecture.Domain.Roles;
using CleanArchitecture.Domain.UnitTests.Infrastructure;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Users.Events;
using FluentAssertions;

namespace CleanArchitecture.Domain.UnitTests.Users;

public class UserTests : BaseTests
{
    [Fact]
    public void Create_Should_SetPropertyValues()
    {
        //Arrange

        //Act
        var user = User.Create(
            UserMock.Nombre,
            UserMock.Apellido,
            UserMock.Email,
            UserMock.Password
        );

        //Assert
        user.Nombre.Should().Be(UserMock.Nombre);
        user.Apellido.Should().Be(UserMock.Apellido);
        user.Email.Should().Be(UserMock.Email);
        user.PasswordHash.Should().Be(UserMock.Password);
    }

    [Fact]
    public void Create_Should_RaiseUserCreateDomainEvent()
    {
        //Arrange

        //Act
        var user = User.Create(
            UserMock.Nombre,
            UserMock.Apellido,
            UserMock.Email,
            UserMock.Password
        );
        //var domainEvent = user.GetDomainEvents().OfType<UserCreatedDomainEvent>().SingleOrDefault();
        var domainEvent = AssertDomainEventWasPublished<UserCreatedDomainEvent>(user);

        //Assert
        domainEvent!.UserId.Should().Be(user.Id);
    }

    [Fact]
    public void Create_Should_AddRegisterRoleToUser()
    {
        //Arrange

        //Act
        var user = User.Create(
            UserMock.Nombre,
            UserMock.Apellido,
            UserMock.Email,
            UserMock.Password
        );

        //Assert
        user.Roles.Should().Contain(Role.Cliente);
    }
}
