using Asp.Versioning;
using CleanArchitecture.Api.Versioning;
using CleanArchitecture.Application.Vehiculos.GetVehiculosByPagination;
using CleanArchitecture.Application.Vehiculos.ReportVehiculoPdf;
using CleanArchitecture.Application.Vehiculos.SearchVehiculos;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Permissions;
using CleanArchitecture.Domain.Vehiculos;
using CleanArchitecture.Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;

namespace CleanArchitecture.Api.Controllers.Vehiculos;

[ApiController]
[Route("api/v{version:apiVersion}/vehiculos")]
[ApiVersion(ApiSupportedVersions.V1)]
public class VehiculosController : ControllerBase
{
    private readonly ISender _sender;

    public VehiculosController(ISender sender)
    {
        _sender = sender;
    }

    [HasPermission(PermissionEnum.ReadUser)]
    [HttpGet("search")]
    [MapToApiVersion(ApiSupportedVersions.V1)]
    public async Task<IActionResult> SearchVehiculos(
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken
    )
    {
        var query = new SearchVehiculosQuery(startDate, endDate);
        var resultados = await _sender.Send(query, cancellationToken);
        return Ok(resultados.Value);
    }

    [AllowAnonymous]
    [HttpGet("getPagination", Name = "PaginationVehiculos")]
    [MapToApiVersion(ApiSupportedVersions.V1)]
    [ProducesResponseType(typeof(PaginationResult<Vehiculo, VehiculoId>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginationResult<Vehiculo, VehiculoId>>> GetPaginationVehiculos(
        [FromQuery] GetVehiculosByPaginationQuery request
    )
    {
        var resultados = await _sender.Send(request);
        return Ok(resultados);
    }

    [AllowAnonymous]
    [HttpGet("reporte")]
    [MapToApiVersion(ApiSupportedVersions.V1)]
    public async Task<IActionResult> ReporteVehiculos(
        CancellationToken cancellationToken,
        string modelo = ""
    )
    {
        var query = new ReportVehiculoPdfQuery(modelo);
        var resultados = await _sender.Send(query, cancellationToken);
        byte[] pdfBytes = resultados.Value.GeneratePdf();
        return File(pdfBytes, "application/pdf");
    }
}
