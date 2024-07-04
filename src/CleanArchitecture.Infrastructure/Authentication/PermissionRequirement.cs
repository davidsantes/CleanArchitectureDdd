using Microsoft.AspNetCore.Authorization;

namespace CleanArchitecture.Infrastructure.Authentication;

/// <summary>
/// Representa un requisito de autorización para verificar si un usuario tiene un permiso específico.
/// </summary>
public class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }

    public string Permission { get; }
}
