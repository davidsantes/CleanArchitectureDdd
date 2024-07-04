using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigracionInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Apellido = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vehiculos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Modelo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Vin = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Direccion_Pais = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Direccion_Departamento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Direccion_Provincia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Direccion_Ciudad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Direccion_Calle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Precio_Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Precio_TipoMoneda = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mantenimiento_Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Mantenimiento_TipoMoneda = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaUltimaAlquiler = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Accesorios = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<long>(type: "bigint", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehiculos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "roles_permissions",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles_permissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_roles_permissions_permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_roles_permissions_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users_roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users_roles", x => new { x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_users_roles_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_users_roles_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "alquileres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehiculoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PrecioPorPeriodo_Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PrecioPorPeriodo_TipoMoneda = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mantenimiento_Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Mantenimiento_TipoMoneda = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Accesorios_Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Accesorios_TipoMoneda = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrecioTotal_Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PrecioTotal_TipoMoneda = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Duracion_Inicio = table.Column<DateOnly>(type: "date", nullable: true),
                    Duracion_Fin = table.Column<DateOnly>(type: "date", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaConfirmacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaDenegacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaCompletado = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaCancelacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alquileres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_alquileres_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_alquileres_vehiculos_VehiculoId",
                        column: x => x.VehiculoId,
                        principalTable: "vehiculos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehiculoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AlquilerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    Comentario = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_reviews_alquileres_AlquilerId",
                        column: x => x.AlquilerId,
                        principalTable: "alquileres",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_reviews_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_reviews_vehiculos_VehiculoId",
                        column: x => x.VehiculoId,
                        principalTable: "vehiculos",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "permissions",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "ReadUser" },
                    { 2, "WriteUser" },
                    { 3, "UpdateUser" }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Cliente" },
                    { 2, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "roles_permissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 2 },
                    { 3, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_alquileres_UserId",
                table: "alquileres",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_alquileres_VehiculoId",
                table: "alquileres",
                column: "VehiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_AlquilerId",
                table: "reviews",
                column: "AlquilerId");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_UserId",
                table: "reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_VehiculoId",
                table: "reviews",
                column: "VehiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_roles_permissions_PermissionId",
                table: "roles_permissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_users_Email",
                table: "users",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_users_roles_UserId",
                table: "users_roles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "roles_permissions");

            migrationBuilder.DropTable(
                name: "users_roles");

            migrationBuilder.DropTable(
                name: "alquileres");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "vehiculos");
        }
    }
}
