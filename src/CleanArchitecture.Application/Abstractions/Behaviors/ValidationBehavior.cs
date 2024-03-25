using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Application.Exceptions;
using FluentValidation;
using MediatR;

namespace CleanArchitecture.Application.Abstractions.Behaviors;

/// <summary>
/// Interceptor que captura las solicitudes de comando antes de ser manejadas por los controladores correspondientes para validar sus datos meciante Fluent Validation.
/// </summary>
/// <typeparam name="TRequest">Tipo de la solicitud del command.</typeparam>
/// <typeparam name="TResponse">Tipo de la respuesta del command.</typeparam>
public class ValidationBehavior<TRequest, TResponse>
: IPipelineBehavior<TRequest, TResponse>
where TRequest : IBaseCommand
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
        )
    {
        // Verificar si hay validadores registrados
        if (!_validators.Any())
        {
            return await next();
        }

        // Crear contexto de validaci�n con la solicitud
        var context = new ValidationContext<TRequest>(request);

        // Ejecutar la validaci�n usando los validadores registrados
        var validationErrors = _validators
            .Select(validators => validators.Validate(context))
            .Where(validationResult => validationResult.Errors.Any())
            .SelectMany(validationResult => validationResult.Errors)
            .Select(validationFailure => new ValidationError(
                validationFailure.PropertyName,
                validationFailure.ErrorMessage
            )).ToList();

        // Si hay errores de validaci�n, lanzar una excepci�n de validaci�n
        if (validationErrors.Any())
        {
            throw new Exceptions.ValidationException(validationErrors);
        }

        // Si no hay errores de validaci�n, continuar con la ejecuci�n normal
        return await next();
    }
}