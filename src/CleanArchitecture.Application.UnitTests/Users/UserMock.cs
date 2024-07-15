using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Application.UnitTests.Users;

internal static class UserMock
{
    public static User Create() => User.Create(Nombre, Apellido, Email, Password);

    public static readonly Nombre Nombre = new("Eduardo");
    public static readonly Apellido Apellido = new("Garcia");
    public static readonly Email Email = new("eduardo.garcia@gmail.com");
    public static readonly PasswordHash Password = new("AfED%%32111");
}
