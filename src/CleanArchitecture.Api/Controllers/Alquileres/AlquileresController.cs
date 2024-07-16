using Asp.Versioning;
using CleanArchitecture.Api.Versioning;
using CleanArchitecture.Application.Alquileres.GetAlquiler;
using CleanArchitecture.Application.Alquileres.ReservarAlquiler;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers.Alquileres;

[ApiController]
[Route("api/v{version:apiVersion}/alquileres")]
[ApiVersion(ApiSupportedVersions.V1)]
public class AlquileresController : ControllerBase
{
    private readonly ISender _sender;

    public AlquileresController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id}")]
    [MapToApiVersion(ApiSupportedVersions.V1)]
    public async Task<IActionResult> GetAlquiler(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetAlquilerQuery(id);
        var resultado = await _sender.Send(query, cancellationToken);

        return resultado.IsSuccess ? Ok(resultado.Value) : NotFound();
    }

    [HttpPost]
    [MapToApiVersion(ApiSupportedVersions.V1)]
    public async Task<IActionResult> ReservaAlquiler(
        Guid id,
        AlquilerReservaRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new ReservarAlquilerCommand(
            request.VehiculoId,
            request.UserId,
            request.StartDate,
            request.EndDate
        );

        var resultado = await _sender.Send(command, cancellationToken);

        if (resultado.IsFailure)
        {
            return BadRequest(resultado.Error);
        }

        return CreatedAtAction(nameof(GetAlquiler), new { id = resultado.Value }, resultado.Value);
    }
}
