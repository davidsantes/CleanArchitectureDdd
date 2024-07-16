using System.Text;
using CleanArchitecture.Application.Abstractions.Data;
using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Application.Vehiculos.SearchVehiculos;
using CleanArchitecture.Domain.Abstractions;
using Dapper;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CleanArchitecture.Application.Vehiculos.ReportVehiculoPdf;

/// <summary>
/// Clase que maneja la generación del PDF para el reporte de vehículos.
/// Implementa la interfaz IQueryHandler para manejar el ReportVehiculoPdfQuery.
/// </summary>
internal sealed class ReportVehiculoPdfQueryHandler
    : IQueryHandler<ReportVehiculoPdfQuery, Document>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    // Constructor que inyecta la fábrica de conexiones SQL.
    public ReportVehiculoPdfQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    // Método encargado de manejar la consulta y generar el documento PDF.
    public async Task<Result<Document>> Handle(
        ReportVehiculoPdfQuery request,
        CancellationToken cancellationToken
    )
    {
        // Crea una nueva conexión a la base de datos usando la fábrica de conexiones.
        using var connection = _sqlConnectionFactory.CreateConnection();

        // Construye la consulta SQL para obtener los vehículos.
        var sqlQuery = BuildSqlQuery(request.Modelo);

        // Ejecuta la consulta SQL y obtiene los resultados.
        var vehiculos = await connection.QueryAsync<VehiculoResponse>(
            sqlQuery.Query,
            new { Search = sqlQuery.Search } // Pasa el parámetro de búsqueda a la consulta.
        );

        // Crea el documento PDF usando QuestPDF.
        var report = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(50); // Establece un margen de 50 unidades en todos los lados.
                page.Size(PageSizes.A4.Landscape()); // Establece el tamaño de la página a A4 en modo paisaje.
                page.PageColor(Colors.White); // Establece el color de fondo de la página a blanco.
                page.DefaultTextStyle(x => x.FontSize(12)); // Establece el tamaño de fuente predeterminado a 12 puntos.

                // Define el encabezado de la página.
                page.Header()
                    .AlignCenter() // Alinea el texto al centro.
                    .Text("Vehiculos: Modernos de Alta Gama") // Texto del encabezado.
                    .SemiBold() // Aplica un estilo de fuente semi-negrita.
                    .FontSize(24) // Establece el tamaño de fuente a 24 puntos.
                    .FontColor(Colors.Blue.Darken2); // Establece el color de la fuente a un azul oscuro.

                // Define el contenido de la página.
                page.Content()
                    .Padding(25) // Agrega un relleno de 25 unidades alrededor del contenido.
                    .Table(table =>
                    {
                        // Define las columnas de la tabla.
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(); // Columna para el modelo.
                            columns.RelativeColumn(); // Columna para el VIN.
                            columns.RelativeColumn(); // Columna para el precio.
                        });

                        // Define el encabezado de la tabla.
                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Modelo"); // Encabezado de la columna "Modelo".
                            header.Cell().Element(CellStyle).Text("Vin"); // Encabezado de la columna "Vin".
                            header.Cell().Element(CellStyle).AlignRight().Text("Precio"); // Encabezado de la columna "Precio" con texto alineado a la derecha.

                            // Define el estilo de celda para el encabezado.
                            static IContainer CellStyle(IContainer container)
                            {
                                return container
                                    .DefaultTextStyle(x => x.SemiBold()) // Estilo de texto semi-negrita.
                                    .PaddingVertical(5) // Agrega un relleno vertical de 5 unidades.
                                    .BorderBottom(1) // Añade una línea en la parte inferior de la celda.
                                    .BorderColor(Colors.Black); // Establece el color de la línea a negro.
                            }
                        });

                        // Agrega las filas de datos a la tabla.
                        foreach (var vehiculo in vehiculos)
                        {
                            table.Cell().Element(CellStyle).Text(vehiculo.Modelo); // Celda para el modelo del vehículo.
                            table.Cell().Element(CellStyle).Text(vehiculo.Vin); // Celda para el VIN del vehículo.
                            table
                                .Cell()
                                .Element(CellStyle)
                                .AlignRight() // Alinea el texto a la derecha.
                                .Text($"${vehiculo.Precio}"); // Celda para el precio del vehículo, con formato de moneda.

                            // Define el estilo de celda para las filas de datos.
                            static IContainer CellStyle(IContainer container)
                            {
                                return container
                                    .BorderBottom(1) // Añade una línea en la parte inferior de la celda.
                                    .BorderColor(Colors.Grey.Lighten2) // Establece el color de la línea a gris claro.
                                    .PaddingVertical(5); // Agrega un relleno vertical de 5 unidades.
                            }
                        }
                    });
            });
        });

        // Devuelve el documento PDF generado como resultado de la consulta.
        return report;
    }

    // Método para construir la consulta SQL.
    private (string Query, string Search) BuildSqlQuery(string modelo)
    {
        var builder = new StringBuilder(
            """
            SELECT
                v.id as Id,
                v.modelo as Modelo,
                v.vin as Vin,
                v.precio_monto as Precio
            FROM vehiculos AS v
            """
        );

        var search = string.Empty;
        if (!string.IsNullOrEmpty(modelo))
        {
            search = "%" + modelo + "%";
            builder.AppendLine(" WHERE v.modelo LIKE @Search");
        }

        builder.AppendLine(" ORDER BY v.modelo ");

        return (builder.ToString(), search);
    }
}
