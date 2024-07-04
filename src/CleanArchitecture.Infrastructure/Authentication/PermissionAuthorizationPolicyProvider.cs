using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Infrastructure.Authentication;

/// <summary>
/// Clase que proporciona una política de autorización personalizada basada en permisos.
/// </summary>
public class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        : base(options) { }

    /// <summary>
    /// Obtiene una política de autorización basada en el nombre de la política.
    /// </summary>
    /// <param name="policyName">El nombre de la política.</param>
    /// <returns>La política de autorización correspondiente, o una nueva política basada en permisos si no se encuentra una existente.</returns>
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        AuthorizationPolicy? policy = await base.GetPolicyAsync(policyName);

        // Si se encuentra una política existente, la devuelve.
        if (policy is not null)
        {
            return policy;
        }

        // Si no se encuentra una política existente, crea una nueva política basada en permisos.
        return new AuthorizationPolicyBuilder()
            .AddRequirements(new PermissionRequirement(policyName))
            .Build();
    }
}
