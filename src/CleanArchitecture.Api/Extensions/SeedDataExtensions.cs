using Bogus;
using CleanArchitecture.Application.Abstractions.Data;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehiculos;
using CleanArchitecture.Infrastructure;
using Dapper;

namespace CleanArchitecture.Api.Extensions;

public static class SeedDataExtensions
{
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
                    new Nombre("Vaxi"),
                    new Apellido("Pérez"),
                    new Email("aaa@aaa.com"),
                    new PasswordHash(passwordHash)
                );

                context.Add(user);

                passwordHash = BCrypt.Net.BCrypt.HashPassword("Admin123$");

                user = User.Create(
                    new Nombre("Admin"),
                    new Apellido("Admin"),
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
        var sqlConnectionFactory =
            scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
        using var connection = sqlConnectionFactory.CreateConnection();

        var faker = new Faker();

        List<object> vehiculos = new();

        for (var i = 0; i < 100; i++)
        {
            vehiculos.Add(
                new
                {
                    Id = Guid.NewGuid(),
                    Vin = faker.Vehicle.Vin(),
                    Modelo = faker.Vehicle.Model(),
                    Pais = faker.Address.Country(),
                    Departamento = faker.Address.State(),
                    Provincia = faker.Address.County(),
                    Ciudad = faker.Address.City(),
                    Calle = faker.Address.StreetAddress(),
                    PrecioMonto = faker.Random.Decimal(1000, 20000),
                    PrecioTipoMoneda = "USD",
                    PrecioMantenimiento = faker.Random.Decimal(100, 200),
                    PrecioMantenimientoTipoMoneda = "USD",
                    Accesorios = new List<int> { (int)Accesorio.Wifi, (int)Accesorio.AppleCar },
                    FechaUltima = DateTime.MinValue
                }
            );
        }

        const string sqlVehiculos = """
                INSERT INTO vehiculos (id, vin, modelo, direccion_pais, direccion_departamento, direccion_provincia, direccion_ciudad, direccion_calle, precio_monto, precio_tipo_moneda, mantenimiento_monto, mantenimiento_tipo_moneda, accesorios, fecha_ultima_alquiler)
                    values(@id, @Vin, @Modelo, @Pais, @Departamento, @Provincia, @Ciudad, @Calle, @PrecioMonto, @PrecioTipoMoneda, @PrecioMantenimiento, @PrecioMantenimientoTipoMoneda, @Accesorios, @FechaUltima)
            """;

        connection.Execute(sqlVehiculos, vehiculos);
    }
}
