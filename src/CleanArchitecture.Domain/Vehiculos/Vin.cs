namespace CleanArchitecture.Domain.Vehiculos;

/// <summary>
/// Value object de n�mero de identificaci�n del coche (Vehicle Identification Number). Una vez creado, no cambia de valor.
/// </summary>
public record Vin(string Value);