using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehiculos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configurations;

/// <summary>
/// Configuraci�n de mapeo de entidad para la entidad Alquiler en la base de datos.
/// </summary>
internal sealed class AlquilerConfiguration : IEntityTypeConfiguration<Alquiler>
{
    public void Configure(EntityTypeBuilder<Alquiler> builder)
    {
        // Configuraci�n de la tabla y clave primaria
        builder.ToTable("alquileres");
        builder.HasKey(alquiler => alquiler.Id);

        // Configuraci�n de mapeo para la propiedad PrecioPorPeriodo (1-1)
        builder.OwnsOne(alquiler => alquiler.PrecioPorPeriodo, precioBuilder =>
        {
            precioBuilder.Property(moneda => moneda.TipoMoneda)
           .HasConversion(tipoMoneda => tipoMoneda.Codigo, codigo => TipoMoneda.FromCodigo(codigo!));
        });

        // Configuraci�n de mapeo para la propiedad Mantenimiento (1-1)
        builder.OwnsOne(alquiler => alquiler.Mantenimiento, precioBuilder =>
        {
            precioBuilder.Property(moneda => moneda.TipoMoneda)
           .HasConversion(tipoMoneda => tipoMoneda.Codigo, codigo => TipoMoneda.FromCodigo(codigo!));
        });

        // Configuraci�n de mapeo para la propiedad Accesorios (1-1)
        builder.OwnsOne(alquiler => alquiler.Accesorios, precioBuilder =>
        {
            precioBuilder.Property(moneda => moneda.TipoMoneda)
           .HasConversion(tipoMoneda => tipoMoneda.Codigo, codigo => TipoMoneda.FromCodigo(codigo!));
        });

        // Configuraci�n de mapeo para la propiedad PrecioTotal (1-1)
        builder.OwnsOne(alquiler => alquiler.PrecioTotal, precioBuilder =>
        {
            precioBuilder.Property(moneda => moneda.TipoMoneda)
           .HasConversion(tipoMoneda => tipoMoneda.Codigo, codigo => TipoMoneda.FromCodigo(codigo!));
        });

        // Configuraci�n de mapeo para la propiedad Duracion (1-1)
        builder.OwnsOne(alquiler => alquiler.Duracion);

        // Configuraci�n de la relaci�n con la entidad Vehiculo (N-1)
        builder.HasOne<Vehiculo>()
            .WithMany()
            .HasForeignKey(alquiler => alquiler.VehiculoId);

        // Configuraci�n de la relaci�n con la entidad User (N-1)
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(alquiler => alquiler.UserId);
    }
}