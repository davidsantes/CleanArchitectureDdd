using CleanArchitecture.Api.Middleware;
using CleanArchitecture.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// M�todo de extensi�n para aplicar migraciones autom�ticamente al iniciar la aplicaci�n.
    /// </summary>
    public static async void ApplyMigration(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var service = scope.ServiceProvider;
            var loggerFactory = service.GetRequiredService<ILoggerFactory>();

            try
            {
                var context = service.GetRequiredService<ApplicationDbContext>();
                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "Error en migraci�n");
            }
        }
    }

    /// <summary>
    /// M�todo de extensi�n para usar un middleware personalizado para manejar excepciones.
    /// </summary>
    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }

    /// <summary>
    /// M�todo de extensi�n para usar un middleware personalizado para manejar el contexto de la solicitud.
    /// </summary>
    public static void UseRequestContextLogin(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestContextLoggingMiddleware>();
    }
}
