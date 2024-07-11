namespace CleanArchitecture.Api.Middleware;

using Serilog.Context;

/// <summary>
/// Clase para manejar el contexto de la solicitud HTTP.
/// Este middleware agrega un identificador de correlación a cada solicitud HTTP para facilitar el seguimiento y la depuración.
/// </summary>
public class RequestContextLoggingMiddleware
{
    // Nombre del encabezado HTTP que contiene el identificador de correlación.
    private const string CorrelationIdHeaderName = "X-Correlation-Id";

    private readonly RequestDelegate _next;

    public RequestContextLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Método invocado para procesar la solicitud HTTP.
    /// </summary>
    /// <param name="httpContext">El contexto de la solicitud HTTP.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    public Task Invoke(HttpContext httpContext)
    {
        using (LogContext.PushProperty("CorrelationId", GetCorrelationId(httpContext)))
        {
            return _next(httpContext);
        }
    }

    /// <summary>
    /// Obtiene el identificador de correlación de la solicitud HTTP.
    /// </summary>
    /// <param name="httpContext">El contexto de la solicitud HTTP.</param>
    /// <returns>El identificador de correlación.</returns>
    private static string GetCorrelationId(HttpContext httpContext)
    {
        httpContext.Request.Headers.TryGetValue(CorrelationIdHeaderName, out var correlationId);

        return correlationId.FirstOrDefault() ?? httpContext.TraceIdentifier;
    }
}
