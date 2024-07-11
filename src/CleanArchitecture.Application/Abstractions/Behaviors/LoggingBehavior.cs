using CleanArchitecture.Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace CleanArchitecture.Application.Abstractions.Behaviors;

/// <summary>
/// Interceptor que captura todos los request que envíe el cliente, al insertar un nuevo record de tipo Command / Query
/// que implementen IBaseRequest.
/// Comportamiento para registrar información de log al ejecutar request (IBaseRequest).
/// </summary>
/// <typeparam name="TRequest">Tipo de la solicitud del request.</typeparam>
/// <typeparam name="TResponse">Tipo de la respuesta del request.</typeparam>
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest
    where TResponse : Result
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var name = request.GetType().Name;

        try
        {
            _logger.LogInformation($"Registro del request {name} antes de ejecutarse", name);
            var result = await next();

            if (result.IsSuccess)
            {
                _logger.LogInformation($"El request {name} fue exitoso", name);
            }
            else
            {
                using (LogContext.PushProperty("Error", result.Error, true))
                {
                    _logger.LogError($"El request {name} tiene errores", name);
                }
            }

            return result;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"El request {name} tiene errores", name);
            throw;
        }
    }
}
