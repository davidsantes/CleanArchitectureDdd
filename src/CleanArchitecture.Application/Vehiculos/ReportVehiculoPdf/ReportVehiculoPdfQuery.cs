using CleanArchitecture.Application.Abstractions.Messaging;
using QuestPDF.Fluent;

namespace CleanArchitecture.Application.Vehiculos.ReportVehiculoPdf;

/// <summary>
/// Declaración de una consulta inmutable (record) para generar un reporte PDF de vehículos.
/// La consulta implementa la interfaz IQuery<Document>, indicando que devolverá un documento PDF.
/// </summary>
/// <param name="Modelo">Esta consulta recibe un parámetro 'Modelo' que se usará para filtrar los vehículos por su modelo.</param>
public sealed record ReportVehiculoPdfQuery(string Modelo) : IQuery<Document>;
