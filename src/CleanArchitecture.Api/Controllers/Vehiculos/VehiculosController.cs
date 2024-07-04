using CleanArchitecture.Application.Vehiculos.SearchVehiculos;
using CleanArchitecture.Domain.Permissions;
using CleanArchitecture.Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers.Vehiculos;

[ApiController]
[Route("api/vehiculos")]
public class VehiculosController : ControllerBase
{
    private readonly ISender _sender;

    public VehiculosController(ISender sender)
    {
        _sender = sender;
    }

    [HasPermission(PermissionEnum.ReadUser)]
    [HttpGet("search")]
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
}
