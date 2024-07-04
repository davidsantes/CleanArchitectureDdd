using CleanArchitecture.Application.Abstractions.Clock;
using CleanArchitecture.Application.Abstractions.Data;
using CleanArchitecture.Application.Abstractions.Email;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehiculos;
using CleanArchitecture.Infrastructure.Clock;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Repositories;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IEmailService, EmailService>();

        // Configuraci�n del contexto de la base de datos utilizando SQL Server
        services.Configure<DatabaseEntityFrameworkOptions>(
            configuration.GetSection(DatabaseEntityFrameworkOptions.SectionName)
        );

        var databaseEntityFrameworkOptions = configuration
            .GetSection(DatabaseEntityFrameworkOptions.SectionName)
            .Get<DatabaseEntityFrameworkOptions>();

        services.AddDbContext<ApplicationDbContext>(
            (serviceProvider, dbContextOptionBuilder) =>
            {
                dbContextOptionBuilder.UseSqlServer(
                    databaseEntityFrameworkOptions!.ConnectionString,
                    sqlServerAction =>
                    {
                        sqlServerAction.EnableRetryOnFailure(
                            databaseEntityFrameworkOptions.MaxRetryCount
                        );
                        sqlServerAction.CommandTimeout(
                            databaseEntityFrameworkOptions.CommandTimeout
                        );
                    }
                );

                //S�lo para entornos de debug, ya que baja el rendimiento y expone datos sensibles.
                dbContextOptionBuilder.EnableDetailedErrors(
                    databaseEntityFrameworkOptions.EnableDetailedErrors
                );
                dbContextOptionBuilder.EnableSensitiveDataLogging(
                    databaseEntityFrameworkOptions.EnableSensitiveDataLogging
                );
            }
        );

        // Registro de implementaciones de repositorios como servicios de �mbito:
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IVehiculoRepository, VehiculoRepository>();
        services.AddScoped<IAlquilerRepository, AlquilerRepository>();

        // Registro de implementaci�n de IUnitOfWork como servicio de �mbito, utilizando el contexto de la base de datos
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        // Registro de implementaci�n de ISqlConnectionFactory como servicio singleton, utilizando la cadena de conexi�n a la base de datos
        services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(
            databaseEntityFrameworkOptions!.ConnectionString
        ));

        // Registro de un manejador de tipo personalizado para Dapper para el tipo DateOnly:
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

        return services;
    }
}
