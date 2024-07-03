using CleanArchitecture.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configurations;

public sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("user_roles");
        builder.HasKey(x => new { x.RoleId, x.UserId });
        builder
            .Property(user => user.UserId)
            .HasConversion(userId => userId!.Value, value => new UserId(value));
    }
}
