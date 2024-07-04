using System.Text.Json;
using Bogus;
using CleanArchitecture.Application.Abstractions.Data;
using CleanArchitecture.Domain.Roles;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehiculos;
using CleanArchitecture.Infrastructure;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Api.Extensions;

public static class SeedDataExtensions
{
    public const string NombreCliente1 = "Cliente nombre";
    public const string NombreAdmin1 = "Admin nombre";

    public const string RolCliente = "Cliente";
    public const string RolAdmin = "Admin";

    /// <summary>
    /// Método de extensión para sembrar datos ficticios de usuarios en la base de datos.
    /// </summary>
    public static void SeedDataUsers(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var service = scope.ServiceProvider;
        var loggerFactory = service.GetRequiredService<ILoggerFactory>();

        try
        {
            var context = service.GetRequiredService<ApplicationDbContext>();

            if (!context.Set<User>().Any())
            {
                var passwordHash = BCrypt.Net.BCrypt.HashPassword("Test123$");

                var user = User.Create(
                    new Nombre(NombreCliente1),
                    new Apellido("Cliente apellido"),
                    new Email("cliente@aaa.com"),
                    new PasswordHash(passwordHash)
                );

                context.Add(user);

                passwordHash = BCrypt.Net.BCrypt.HashPassword("Admin123$");

                user = User.Create(
                    new Nombre(NombreAdmin1),
                    new Apellido("Admin apellido"),
                    new Email("admin@aaa.com"),
                    new PasswordHash(passwordHash)
                );

                context.Add(user);

                context.SaveChangesAsync().Wait();
            }
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<ApplicationDbContext>();
            logger.LogError(ex.Message);
        }
    }

    /// <summary>
    /// Método de extensión para sembrar datos ficticios de vehículos en la base de datos.
    /// </summary>
    public static void SeedDataVehiculos(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var service = scope.ServiceProvider;
        var loggerFactory = service.GetRequiredService<ILoggerFactory>();

        var sqlConnectionFactory =
            scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
        using var connection = sqlConnectionFactory.CreateConnection();

        var faker = new Faker();

        List<object> vehiculos = new();

        DateTime startDate = new(2000, 1, 1); // Fecha de inicio del rango
        DateTime endDate = new(2024, 1, 1); // Fecha de fin del rango

        for (var i = 0; i < 100; i++)
        {
            vehiculos.Add(
                new
                {
                    Id = Guid.NewGuid(),
                    Vin = faker.Vehicle.Vin(),
                    Modelo = faker.Vehicle.Model(),
                    Direccion_Pais = faker.Address.Country(),
                    Direccion_Departamento = faker.Address.State(),
                    Direccion_Provincia = faker.Address.County(),
                    Direccion_Ciudad = faker.Address.City(),
                    Direccion_Calle = faker.Address.StreetAddress(),
                    Precio_Monto = faker.Random.Decimal(1000, 20000),
                    Precio_TipoMoneda = "USD",
                    Mantenimiento_Monto = faker.Random.Decimal(100, 200),
                    Mantenimiento_TipoMoneda = "USD",
                    Accesorios = JsonSerializer.Serialize(
                        new List<int> { (int)Accesorio.Wifi, (int)Accesorio.AppleCar }
                    ),
                    FechaUltimaAlquiler = faker.Date.Between(startDate, endDate),
                    Version = 0 // Agregar la versión inicial
                }
            );
        }

        const string sqlVehiculos =
            @"
            INSERT INTO [dbo].[vehiculos] (
                [Id], [Vin], [Modelo], [Direccion_Pais], [Direccion_Departamento], [Direccion_Provincia],
                [Direccion_Ciudad], [Direccion_Calle], [Precio_Monto], [Precio_TipoMoneda], [Mantenimiento_Monto],
                [Mantenimiento_TipoMoneda], [Accesorios], [FechaUltimaAlquiler], [Version]
            )
            VALUES (
                @Id, @Vin, @Modelo, @Direccion_Pais, @Direccion_Departamento, @Direccion_Provincia,
                @Direccion_Ciudad, @Direccion_Calle, @Precio_Monto, @Precio_TipoMoneda, @Mantenimiento_Monto,
                @Mantenimiento_TipoMoneda, @Accesorios, @FechaUltimaAlquiler, @Version
            )";

        try
        {
            connection.Execute(sqlVehiculos, vehiculos);
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<ApplicationDbContext>();
            logger.LogError(ex.Message);
        }
    }

    public static async Task SeedDataUsersRoles(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var service = scope.ServiceProvider;
        var loggerFactory = service.GetRequiredService<ILoggerFactory>();

        try
        {
            var context = service.GetRequiredService<ApplicationDbContext>();

            var users = context.Set<User>().ToList();
            var roles = context.Set<Role>().ToList();
            var userRoles = context.Set<UserRole>().ToList();

            // Existen datos maestros pero no se han asignado roles a los usuarios
            if (users.Any() && roles.Any() && !userRoles.Any())
            {
                var userCliente = users.FirstOrDefault(x => x.Nombre!.Value == NombreCliente1);
                var clienteRole = new UserRole { UserId = userCliente!.Id, RoleId = 1 };
                context.Add(clienteRole);

                var userAdmin = users.FirstOrDefault(x => x.Nombre!.Value == NombreAdmin1);
                var adminRole = new UserRole { UserId = userAdmin!.Id, RoleId = 2 };
                context.Add(adminRole);

                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<ApplicationDbContext>();
            logger.LogError(ex.Message);
        }
    }
}
