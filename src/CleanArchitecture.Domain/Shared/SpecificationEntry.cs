namespace CleanArchitecture.Domain.Shared;

/// <summary>
/// Clase que representa una entrada de especificación de consulta.
/// </summary>
public record SpecificationEntry
{
    public string? SortBy { get; set; }
    public int PageIndex { get; set; } = 1;

    private const int maxPageSize = 50;
    private int _pageSize = 20;

    /// <summary>
    /// Tamaño de la página de resultados. Controla que la cantidad de elementos no sea mayor a <see cref="maxPageSize"/>.
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
    }

    public string? Search { get; set; }
}
