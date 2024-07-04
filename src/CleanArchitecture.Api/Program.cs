using CleanArchitecture.Api.Extensions;
using CleanArchitecture.Api.OptionsSetup;
using CleanArchitecture.Application;
using CleanArchitecture.Application.Abstractions.Authentication;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//Configuración de la autenticación
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
builder.Services.AddTransient<IJwtProvider, JwtProvider>();

//Configuración de la autorización
builder.Services.AddAuthorization();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddSingleton<
    IAuthorizationPolicyProvider,
    PermissionAuthorizationPolicyProvider
>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Aplica las migraciones, comentar si no se quiere ejecutar (excluyente de SeedData..., primero lanzar ApplyMigration y luego la inserción de datos):
app.ApplyMigration();

//Inserta datos ficticios, comentar si no se quiere ejecutar:
app.SeedDataUsers();
app.SeedDataUsersRoles().Wait();
app.SeedDataVehiculos();

app.UseCustomExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
