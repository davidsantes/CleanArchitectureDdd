using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CleanArchitecture.Api.Versioning;

/// <summary>
/// Configura las opciones de Swagger para versionado de la API.
/// </summary>
public sealed class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    public void Configure(SwaggerGenOptions options)
    {
        //Al swagger le añadimos la documentación de cada versión
        foreach (var documentation in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(documentation.GroupName, CreateDocumentation(documentation));
        }
    }

    /// <summary>
    /// Crea la documentación de OpenAPI para una descripción de versión de API.
    /// </summary>
    /// <param name="apiVersionDescription">La descripción de la versión de la API.</param>
    /// <returns>Un objeto <see cref="OpenApiInfo"/> con la información de la documentación.</returns>
    private static OpenApiInfo CreateDocumentation(ApiVersionDescription apiVersionDescription)
    {
        var openApiInfo = new OpenApiInfo
        {
            Title = $"CleanArchitecture.Api v{apiVersionDescription.ApiVersion}",
            Version = apiVersionDescription.ApiVersion.ToString(),
        };

        if (apiVersionDescription.IsDeprecated)
        {
            openApiInfo.Description += "Esta API version ha sido deprecada";
        }

        return openApiInfo;
    }
}
