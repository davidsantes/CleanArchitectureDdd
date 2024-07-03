using CleanArchitecture.Domain.Permissions;

namespace CleanArchitecture.Domain.Roles;

public sealed class RolePermission
{
    public int RoleId { get; set; }
    public PermissionId? PermissionId { get; set; }
}
