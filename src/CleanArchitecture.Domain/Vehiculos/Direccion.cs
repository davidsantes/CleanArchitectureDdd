namespace CleanArchitecture.Domain.Vehiculos;

/// <summary>
/// Value object de direcci�n. Una vez creado, no cambia de valor.
/// </summary>
public record Direccion
(
    string Pais,
    string Departamento,
    string Provincia,
    string Ciudad,
    string Calle
);