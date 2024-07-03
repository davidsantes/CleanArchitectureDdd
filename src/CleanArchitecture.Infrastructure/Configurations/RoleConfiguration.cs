using CleanArchitecture.Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");
        builder.HasKey(x => x.Id);
        // Alimentar la tabla 'roles' con los datos de los roles
        builder.HasData(Role.GetValues());
        // Configuración de la relación muchos a muchos con la entidad Permission
        builder.HasMany(x => x.Permissions).WithMany().UsingEntity<RolePermission>();
    }
}
