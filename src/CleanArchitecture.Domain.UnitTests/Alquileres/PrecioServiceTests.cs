using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Domain.UnitTests.Vehiculos;
using FluentAssertions;

namespace CleanArchitecture.Domain.UnitTests.Alquileres;

public class PrecioServiceTests
{
    [Fact]
    public void CalcularPrecio_Should_ReturnCorrectPrecioTotal()
    {
        //Arrange
        var precio = new Moneda(10.0m, TipoMoneda.Usd);

        var periodo = DateRange.Create(new DateOnly(2024, 1, 1), new DateOnly(2025, 1, 1));

        var expectedPrecioTotal = new Moneda(
            precio.Monto * periodo.CantidadDias,
            precio.TipoMoneda
        );

        var vehiculo = VehiculoMock.Create(precio);
        var precioService = new PrecioService();

        //Act
        var precioDetalle = precioService.CalcularPrecio(vehiculo, periodo);

        //Assert
        precioDetalle.PrecioTotal.Should().Be(expectedPrecioTotal);
    }

    [Fact]
    public void CalcularPrecio_Should_ReturnCorrectPrecioTotal_WhenMantenimientoIsIncluded()
    {
        //Arrange
        var precio = new Moneda(10.0m, TipoMoneda.Usd);
        var precioMantenimiento = new Moneda(100.00m, TipoMoneda.Usd);

        var periodo = DateRange.Create(new DateOnly(2024, 1, 1), new DateOnly(2025, 1, 1));

        var expectedPrecioTotal = new Moneda(
            (precio.Monto * periodo.CantidadDias) + precioMantenimiento.Monto,
            precio.TipoMoneda
        );

        var vehiculo = VehiculoMock.Create(precio, precioMantenimiento);
        var precioService = new PrecioService();

        //Act
        var precioDetalle = precioService.CalcularPrecio(vehiculo, periodo);

        //Assert
        precioDetalle.PrecioTotal.Should().Be(expectedPrecioTotal);
    }
}
