using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace CleanArchitecture.Infrastructure.Authentication;

/// <summary>
/// Representa un manejador de autorización para verificar si un usuario tiene un permiso específico.
/// </summary>
public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement
    )
    {
        string? userId = context
            .User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)
            ?.Value;

        if (userId is null)
        {
            return Task.CompletedTask;
        }

        HashSet<string> permisssions = context
            .User.Claims.Where(x => x.Type == CustomClaims.Permissions)
            .Select(x => x.Value)
            .ToHashSet();

        if (permisssions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
