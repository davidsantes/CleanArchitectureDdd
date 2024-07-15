using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Alquileres.Events;
using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Domain.UnitTests.Infrastructure;
using CleanArchitecture.Domain.UnitTests.Users;
using CleanArchitecture.Domain.UnitTests.Vehiculos;
using CleanArchitecture.Domain.Users;
using FluentAssertions;

namespace CleanArchitecture.Domain.UnitTests.Alquileres;

public class AlquilerTests : BaseTests
{
    [Fact]
    public void Reserva_Should_RaiseAlquilerReservaDomainEvent()
    {
        //Arrange
        var user = User.Create(
            UserMock.Nombre,
            UserMock.Apellido,
            UserMock.Email,
            UserMock.Password
        );
        var precio = new Moneda(10.0m, TipoMoneda.Usd);
        var periodo = DateRange.Create(new DateOnly(2024, 1, 1), new DateOnly(2025, 1, 1));
        var vehiculo = VehiculoMock.Create(precio);
        var precioService = new PrecioService();

        //Act
        var alquiler = Alquiler.Reservar(
            vehiculo,
            user.Id!,
            periodo,
            DateTime.UtcNow,
            precioService
        );

        //Assert
        var alquilerReservaDomainEvent =
            AssertDomainEventWasPublished<AlquilerReservadoDomainEvent>(alquiler);
        alquilerReservaDomainEvent.AlquilerId.Should().Be(alquiler.Id);
    }
}
