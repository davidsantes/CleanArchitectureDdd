using CleanArchitecture.Domain.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace CleanArchitecture.Infrastructure.Authentication;

/// <summary>
/// Representa un atributo de autorización para verificar si un usuario tiene un permiso específico.
/// </summary>
public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(PermissionEnum permission)
        : base(policy: permission.ToString())
    {
        Policy = permission.ToString();
    }
}
