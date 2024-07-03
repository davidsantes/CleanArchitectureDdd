using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Permissions;

public sealed class Permission : Entity<PermissionId>
{
    private Permission() { }

    public Permission(PermissionId id, Nombre nombre)
        : base(id)
    {
        Nombre = nombre;
    }

    public Permission(Nombre nombre)
        : base()
    {
        Nombre = nombre;
    }

    public Nombre? Nombre { get; init; }

    public static Result<Permission> Create(Nombre nombre)
    {
        return new Permission(nombre);
    }
}
