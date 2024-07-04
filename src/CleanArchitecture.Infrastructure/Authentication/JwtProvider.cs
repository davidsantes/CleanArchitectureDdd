using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CleanArchitecture.Application.Abstractions.Authentication;
using CleanArchitecture.Application.Abstractions.Data;
using CleanArchitecture.Domain.Users;
using Dapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Infrastructure.Authentication;

public sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public JwtProvider(IOptions<JwtOptions> options, ISqlConnectionFactory sqlConnectionFactory)
    {
        _options = options.Value;
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    /// <summary>
    /// Método para generar un token JWT para un usuario, rellenando con sus datos básicos y permisos.
    /// </summary>
    public async Task<string> Generate(User user)
    {
        //Esta parte de retorno de permisos se podría poner en un service.
        const string sql = """
            SELECT
                p.nombre
            FROM users usr
                LEFT JOIN users_roles usrl
                    ON usr.id = usrl.user_id
                LEFT JOIN roles rl
                    ON rl.id = usrl.role_id
                LEFT JOIN roles_permissions rp
                    ON rl.id = rp.role_id
                LEFT JOIN permissions p
                    ON p.id = rp.permission_id
            WHERE usr.id = @UserId;
            """;

        using var connection = _sqlConnectionFactory.CreateConnection();
        var permissions = await connection.QueryAsync<string>(sql, new { UserId = user.Id!.Value });

        var permissionCollection = permissions.ToHashSet();

        // Crear los claims (reclamaciones) que se incluirán en el token (ID del usuario, email...)
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id!.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!.Value)
        };

        // Se está agregando la lista de permisos al token
        foreach (var permission in permissionCollection)
        {
            claims.Add(new(CustomClaims.Permissions, permission));
        }

        // Crear las credenciales de firma usando la clave secreta y el algoritmo de seguridad
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey!)),
            SecurityAlgorithms.HmacSha256
        );

        // Crear el token JWT con los parámetros especificados
        var token = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            claims,
            null,
            DateTime.UtcNow.AddDays(365),
            signingCredentials
        );

        // Serializar el token a una cadena JWT
        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }
}
