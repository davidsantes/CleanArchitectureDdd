using CleanArchitecture.Domain.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configurations;

public sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");
        builder.HasKey(p => p.Id);
        builder
            .Property(x => x.Id)
            .HasConversion(permissionId => permissionId!.Value, value => new PermissionId(value));

        builder
            .Property(x => x.Nombre)
            .HasConversion(permissionNombre => permissionNombre!.Value, value => new Nombre(value));

        // Alimentar la tabla 'permissions' con los datos de los permisos
        IEnumerable<Permission> permissions = Enum.GetValues<PermissionEnum>()
            .Select(p => new Permission(new PermissionId((int)p), new Nombre(p.ToString())));
        builder.HasData(permissions);
    }
}
