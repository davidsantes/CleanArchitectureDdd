using CleanArchitecture.Application.Abstractions.Messaging;

namespace CleanArchitecture.Application.Vehiculos.SearchVehiculos;

/// <summary>
/// Veh�culos que pueden ser alquilados en un determinado rango de fechas
/// </summary>
public sealed record SearchVehiculosQuery(
    DateOnly fechaInicio,
    DateOnly fechaFin
) : IQuery<IReadOnlyList<VehiculoResponse>>;