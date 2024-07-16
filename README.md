﻿# Clean architecture básico y avanzado

Esta versión contiene los ejercicios tomados de:
* Curso de .Net University en Udemy: **Clean Architecture y Domain Driven Design en ASP.NET Core 8**, y complementado con apuntes propios.
* Curso de .Net University en Udemy: **Clean Architecture y Domain Driven Design en ASP.NET Core 8 avanzado**, y complementado con apuntes propios.

# Clean architecture y Domain Driven Design (básico)

Ejercicios tomados del curso de .Net University en Udemy: **Clean Architecture y Domain Driven Design en ASP.NET Core 8**, y complementado con apuntes propios.

---

# Índice de contenidos básicos 📋
1. **[Clean architecture en .NET](#Seccion_01_Clean)**
2. **[Capa de Domain](#Seccion_02_Capa_Domain)**
3. **[Capa de Application](#Seccion_03_Capa_Application)**
4. **[Capa de Infrastructure](#Seccion_04_Capa_Infrastructure)**
5. **[Capa de presentación - Web API](#Seccion_05_Capa_Presentacion_WebApi)**

# Índice de contenidos avanzados 📋
1. **[SECCIÓN 02. Domain Driven Design con Identificadores avanzados (strong ids)](#Seccion_02_StrongIds)**
2. **[SECCIÓN 03. Modelos de authentication en clean architecture](#Seccion_03_Authentication)**
3. **[SECCIÓN 04. Seguridad y migración de EF en Clean architecture](#Seccion_04_Migracion)**
4. **[SECCIÓN 06. Authorization con permisos y roles en Clean Architecture](#Seccion_06_Authorization)**
5. **[SECCIÓN 07. Authorization en Controllers y Json Web Tokens (JWT)](#Seccion_07_Authorization_Jwt)**
6. **[SECCIÓN 08. Serilog en clean architecture y Net](#Seccion_08_Serilog)**
7. **[SECCIÓN 09. Paginación en Clean Architecture](#Seccion_09_Paginacion)**
8. **[SECCIÓN 12. Versionado en Apis](#Seccion_12_Versionado)**
9. **[SECCIÓN 15. Unit test en Clean architecture](#Seccion_15_Testing_Clean_Architecture)**
10. **[SECCIÓN 21. Reportes con Quest Pdf](#Seccion_21_Reportes)**

---

# Toma de contacto  🚀 <a name="Toma_Contacto"></a>

## Principales puntos 📋
* Creación de un back-end para el alquiler de coches mediante un enfoque de dominio - DDD.
* Estructura de una aplicación basada en Clean Architecture.
* Capa de Domain: organización de la lógica de negocio.
  - Uso de Entity Framework Core para modelar las entidades del dominio.
  - Implementación de eventos de dominio y notificaciones mediante eventos.
  - Creación de servicios de dominio y contratos de acceso a base de datos (repositorios) y persistencia (unit of work).
  - Manejo de objetos de resultados y errores.
* Capa de Application: gestión de casos de uso y lógica de aplicación.
  - Utilización de MediatR para implementar el patrón Mediator y CQRS.
  - Manejo de comandos y consultas.
  - Uso de Dapper para consultas específicas.
* Capa de Infrastructure: implementación de detalles técnicos y externos.
  - Configuración de Entity Framework Core.
  - Implementación de repositorios y acceso a base de datos.
  - Configuración de servicios externos como servicios de correo electrónico.
* Preparación y ejecución de migraciones de base de datos.
* Manejo de excepciones y retorno de errores estandarizados.
* Uso de controladores en una API RESTful.
* Ejecución de la API a través de herramientas como Postman.

## Pre-requisitos 📋
Como herramientas de desarrollo necesitarás:
* Visual Studio 2022 (con la versión para .NET 8)
* SQL Server.

## Antes de comenzar... entiende la base de datos que vamos a utilizar ⚙️
Los ejemplos se realizan sobre una base de datos de alquileres de coches.
![My Image](./docs/imgs/02.Bdd.png)

Además, al ser Code First, existirá una tabla con las migraciones:

| Migración |
|------------|
| ![Imagen 1](./docs/imgs/08.Bdd_Tablas2.JPG)   |


## Agradecimientos 🎁

* Plataforma de aprendizaje online [Udemy](https://www.udemy.com/share/109PRS3@gz4ZDXhSu8i9pa_CnjiahHDgwCptf9vw-CYR0FqedgI2UGsgwy4nmPTe3ehw5QaGMA==/)
* A cualquiera que me invite a una cerveza 🍺.

---

# SECCIÓN 01. Clean architecture en .NET <a name="Seccion_01_Clean"></a>
![My Image](./docs/imgs/00.DDD.JPG)

---

# SECCIÓN 02. Proyecto CleanArchitecture.Domain <a name="Seccion_02_Capa_Domain"></a>

**Se trata del corazón del negocio:**
![My Image](./docs/imgs/01.Domain.JPG)

**Estructura de carpetas:**

![My Image](./docs/imgs/03.CleanArchitecture.Domain.Folders.JPG)

**CleanArchitecture.Domain.Abstractions:**
* `public abstract class Entity`: para identificar entidades, y poner un `Guid` a las clases de tipo entidad. La propiedad tiene como setter `init`,  Init indica que una vez que ha sido inicializada la propiedad, no se puede cambiar su valor.
* `public interface IDomainEvents : INotification`: para configurar eventos de dominio. La entidad base `Entity`, manejará estos eventos.

**Principales características de una entidad de dominio:**
* Clase `sealed`: para que esté sellada.
* Debe tener un identificador, debe heredar de la clase abstracta `Entity`.
* Propiedades con setter `private set`: para cambiar los valores se deberá hacer a través de métodos.
* Constructor privado. Existirá un factory method llamado `Create` para la cración de la clase.

**Creación de Value objects:**
* Ejemplos con records simples: Direccion, Modelo, Vin. Aportan legibilidad al negocio. Representados como records, por lo que no cambian de valor.
* Ejemplos con records complejos: `TipoMoneda`, `Moneda`.

**Creación de eventos de dominio y notificaciones:**
* Para cambios en el estado de una entidad.
* Creados a través del paquete Nuget `MediatR.Contracts`.
* Ejemplos:  `Alquileres/AlquilerCanceladoDomainEvent`, `Users/UserCreatedDomainEvent`.

**Creación de **servicios de dominio**:**
* Un ejemplo es la clase `PrecioService`, para realizar cálculos de precios.

**Creación de los contratos de acceso a base de datos (repositorios) y persistencia (unit of work):**
* Repositorios: ejemplo `IUserRepository`.
* Unit of work: ejemplo `IUnitOfWork`.

**Creación de objetos de Results:**
* Clase `Abstractions/Result`: clase para poder devolver resultados estructurados.
* Clase `Abstractions/Error`: clase para poder devolver errores estructurados. Posteriormente se crearán errores propios de cada dominio (por ejemplo, de tipo `AlquilerErrors`).

**Objetos shared en Domain Model:**
* Dentro del modelo hay componentes que se va a reusar dentro de las entidades.
* Por ejemplo: `Shared/TipoMoneda`, `Shared/Moneda`.

---

# SECCIÓN 03. Proyecto CleanArchitecture.Application <a name="Seccion_03_Capa_Application"></a>

**Estructura de carpetas:**

![My Image](./docs/imgs/04.CleanArchitecture.Application.Folders.JPG)

**Paquetes Nuget:**
* Uso de `MediatR`: MediatR es una implementación del patrón mediador que ocurre completamente en el mismo proceso de la aplicación (in-process), y es una herramienta fundamental para crear sistemas basados en CQRS. Toda la comunicación entre el usuario y la capa de persistencia se gestiona a través de MediatR.
* Uso de `Dapper`.

**Carpeta `Abstractions`, que dentro contiene, entre otros:**
* Carpeta `Behaviours` (Interceptores para *cross cutting concerns): en esta carpeta se encuentran los interceptores:
	* `LoggingBehavior`: interceptor que captura todos los request que envíe el cliente, al insertar un nuevo record de tipo Command que implementen IBaseCommand. Comportamiento para registrar información de log al ejecutar commands (IBaseCommand).
	* `ValidationBehavior`: interceptor que captura las solicitudes de comando antes de ser manejadas por los controladores correspondientes para validar sus datos meciante Fluent Validation.
* Carpeta `Messaging`: en esta carpeta se encuentran las interfaces relacionadas con el manejo de mensajes dentro de la aplicación.
	* Interfaces para **Queries**: 
		* `IQuery`: Define una interfaz para las consultas que devuelven un resultado.
			* Por ejemplo, para devolver todos los usuarios, se crearía la clase `ConsultasUsuarioQuery`.
		* `IQueryHandler`: Define una interfaz para los controladores de consultas.
			* Por ejemplo, para manejar consultas relacionadas con usuarios, se crearía una clase que implemente esta interfaz, como `ConsultasUsuarioQueryHandler`.
	* Interfaces para **Commands**: 
		* `ICommand`: Define una interfaz para los comandos que no devuelven ningún resultado.
			* Por ejemplo, para crear un usuario, se crearía la clase `UsuarioCommand`.
		* `ICommandHandler`: Define una interfaz para los controladores de comandos.
			* Por ejemplo, para manejar comandos relacionados con usuarios, se crearía una clase que implemente esta interfaz, como `UsuarioCommandHandler`.

**Carpeta `Exceptions`, que dentro contiene, entre otros:**
* Clase `ValidationException`, que contiene una lista de errores de tipo `ValidationError`.

**Carpeta `Alquileres`, que dentro contiene la lógica de alquileres, entre otros**:
* Lógica para gestionar alquileres, con sus correspondientes Query, Command, etc.
* Esta lógica tiene encuentra errores de concurrencia `ConcurrencyException`.

**Carpeta `Vehiculos`, que dentro contiene la lógica de vehículos, entre otros**:
* Lógica para buscar vehículos, con sus correspondientes Query.

**Clase `DependencyInjection.cs`, encargada de la inyección de dependencias, por ejemplo, de**:
* Registrar los servicios, como por ejemplo `PrecioService`.
* Registrar los datos necesarios de `MediatR`, como los Command, Queries y sus respectivos Handlers a través del patrón Mediator.

---

# SECCIÓN 04. Proyecto CleanArchitecture.Infrastructure <a name="Seccion_04_Capa_Infrastructure"></a>

**Estructura de carpetas:**

![My Image](./docs/imgs/05.CleanArchitecture.Infrastructure.Folders.JPG)

**Paquetes Nuget:**
* Uso de `Microsoft.EntityFrameworkCore`, `Microsoft.EntityFrameworkCore.Tools`, `Microsoft.EntityFrameworkCore.SqlServer`.

**Carpeta `Configurations`, que dentro contiene, los mapeos de las entidades a las tablas de base de datos.**

**Carpeta `Data`, con las configuraciones necesarias para Dapper:**
* `SqlConnectionFactory`, factoría de conexión que se utilizará para Dapper (consultas). Implementa la interfaz `ISqlConnectionFactory`.
* `DateOnlyTypeHandler`, clase para manejar la conversión entre el tipo de datos .NET DateOnly y el tipo de datos de la base de datos.

**Carpeta `Clock`, que dentro contiene la implementación de la interfaz `IDateTimeProvider`. Esta implementación va a ser:
* `internal`, ya que vamos a utilizar la interfaz, que sí será pública.
* `sealed`, ya que va a estar sellada y no permititrá su herencia.

**Carpeta `Email`, que dentro contiene la implementación de la interfaz `EmailService`. Esta implementación va a ser:
* `internal`, ya que vamos a utilizar la interfaz, que sí será pública.
* `sealed`, ya que va a estar sellada y no permititrá su herencia.
* Se trata de una simulación de envío no implementado.

**Carpeta `Repositories`, que dentro contiene la implementación de las interfaces de dominio `IAlquilerRepository`, `IUserRepository`, etc.

**Clase `ApplicationDbContext.cs`, Clase que representa el contexto de la base de datos de la aplicación, se encarga de**:
* Configurar el modelo de la base de datos.
* Guardar los cambios en la base de datos y publicar los eventos de dominio asociados mediante el publisher de `MediatR`.
* Controlar los *errores por concurrencia optimista*. Para más info, revisar `ReservarAlquilerCommandHandler.cs` y `VehiculoConfiguration.cs` (añade una columna de versión en cada fila para registros optimistas).

**Clase `DependencyInjection.cs`, encargada de la inyección de dependencias, por ejemplo, de**:
* Registrar los servicios, como por ejemplo `DateTimeProvider` y `EmailService`.
* Registro de la base de datos.

---
# SECCIÓN 05. Proyecto CleanArchitecture.Api <a name="Seccion_05_Capa_Presentacion_WebApi"></a>

**Estructura de carpetas:**

![My Image](./docs/imgs/06.CleanArchitecture.Api.Folders.JPG)

## Preparación de la migración para base de datos
Seguir los siguientes pasos para crear una migración inicial:
1. Ir al Package Manager Console y situarse en el proyecto `CleanArchitecture.Api`.
2. Ejecutar:
`
dotnet tool install --global dotnet-ef
`

	El comando `dotnet tool install --global dotnet-ef` se utiliza para instalar la herramienta global `dotnet-ef`. Esta herramienta forma parte de la plataforma .NET Core y se utiliza para interactuar con Entity Framework Core (EF Core) desde la línea de comandos.
	- `dotnet tool install`: Es el comando principal para instalar herramientas .NET Core.
	- `--global`: Especifica que la herramienta se instalará de forma global en el sistema, lo que significa que estará disponible en cualquier directorio y para cualquier proyecto.
	- `dotnet-ef`: Es el nombre de la herramienta que se está instalando, que se utiliza para ejecutar comandos relacionados con Entity Framework Core, como la creación de migraciones y la actualización de la base de datos.

3. Ejecutar:
`
dotnet ef --verbose migrations add MigracionInicial -p src/CleanArchitecture.Infrastructure -s src/CleanArchitecture.Api
`

	Esta línea de comando ejecuta EF Core en un entorno detallado (--verbose) para agregar una migración con el nombre "MigracionInicial" a un proyecto. Aquí está la explicación detallada:
	- `dotnet ef`: Inicia la herramienta CLI (Command Line Interface) de Entity Framework Core.
	- `--verbose`: Esta bandera indica que la salida será detallada, proporcionando más información durante la ejecución.
	- `migrations add MigracionInicial`: Agrega una nueva migración al proyecto actual con el nombre "MigracionInicial". Las migraciones en Entity Framework Core se utilizan para realizar cambios en la estructura de la base de datos y se aplican a través del proceso de migración.
	- `-p src/CleanArchitecture.Infrastructure`: Especifica la ruta del proyecto donde se encuentran los archivos de la infraestructura de la aplicación. En este caso, los archivos del proyecto se encuentran en el directorio `src/CleanArchitecture.Infrastructure`.
	- `-s src/CleanArchitecture.Api`: Especifica la ruta del proyecto de inicio donde se encuentran los archivos de la API de la aplicación. En este caso, los archivos del proyecto de inicio se encuentran en el directorio `src/CleanArchitecture.Api`.

4. Resultado:
Si todo ha ido correctamente, habrá creado una migración como:

![My Image](./docs/imgs/07.CleanArchitecture.Migrations.Folder.JPG)

5. Una vez creadas las migraciones, la clase `program.cs` del proyecto `CleanArchitecture.Api` llamará a:
	- `ApplicationBuilderExtensions.ApplyMigration` para ejecutar las migraciones.
	- `SeedDataExtensions` para generar datos por defecto. Utiliza la librería `Bogus` para introducir datos fake.

6. Ejecutar el proyecto `CleanArchitecture.Api`, y se creará tanto la base de datos como datos en las tablas:
 
![My Image](./docs/imgs/08.Bdd_Tablas1.JPG)

## Captura de excepciones
La aplicación utiliza el Middleware `ExceptionHandlingMiddleware` para capturar las excepciones y mostrarlas de una forma entendible para el cliente.
En caso de que se produzca un error, se devolverá un [ProblemDetails](https://datatracker.ietf.org/doc/html/rfc7807), el cual es un estándar de devolución de errores.

## Los controladores
Existen dos controladores dentro de la API:
* `AlquileresController`: con 1 método get para retornar los alquileres y 1 método post para insertar.
* `VehiculosController`: con 1 método get para retornar los vehículos.

## Ejecución de la api
La ejecución de la Api se puede realizar desde:
* **Postman**: se incluye en la carpeta "docs" un environment y una colección con batería de pruebas de postman. Se debe tener en cuenta que los guids de las consultas quizás no coincidan ya que son autogenerados.
* **Swagger**.


# SECCIÓN 02. Domain Driven Design con Identificadores avanzados (strong ids) <a name="Seccion_02_StrongIds"></a>

**¿Qué son los strong ids?:**
* En el contexto del Domain-Driven Design (DDD), los Strong IDs (identificadores fuertes) son una técnica para gestionar identificadores de entidades de manera segura y expresiva.
* En resumen, en vez de utilizar `Guid`, utilizaremos `UserId`, `AlquilerId`, etcétera.

**¿Qué implicaciones tiene?:**
* Al utilizar strong ids, la generación no es directa. Esto implica, por ejemplo, tener que crear una interfaz `IEntity` para poderla utilizar en la clase `ApplicationDbContext`.

---

# SECCIÓN 03. Modelos de authentication en clean architecture <a name="Seccion_03_Authentication"></a>

APis de autenticación externas para sistemas distribuidos: Okta, Azure (Microsoft Entra ID), KeyCloak, etcétera.
![My Image](./docs/imgs/09.Authentication.JPG)


## Nugets utilizados
- `BCrypt.Net-Next`, en la capa Application. Se trata de un nuget para encriptación / desencriptación de passwords. 
- `Microsoft.AspNetCore.Authentication.JwtBearer`, en la capa Infrastructure. Se trata de Nuget para uso de Json Web Token.

## Conceptos básicos de JWT
![My Image](./docs/imgs/09.Authentication2.JPG)

### Diferencias entre Secret Key, Audience e Issuer

En el contexto de autenticación y seguridad, especialmente en sistemas basados en tokens como JWT (JSON Web Tokens), los términos "secret key", "audience" e "issuer" tienen roles y significados específicos. Aquí están las diferencias entre ellos:

#### Secret Key (Clave Secreta)
- **Propósito**: Es utilizada para firmar y, en algunos casos, para cifrar los tokens. Garantiza que el token no ha sido alterado y confirma la autenticidad del emisor.
- **Uso**: Al crear un JWT, se usa la clave secreta para firmar el token. Cualquier receptor del token puede verificar su autenticidad utilizando la misma clave.
- **Características**: Debe ser mantenida en secreto, sólo conocida por el servidor o servicio que emite y valida los tokens. Si se compromete, los tokens firmados con esa clave pueden ser falsificados.
- **Ejemplo**: `"mySuperSecretKey12345"`

#### Audience (Audiencia)
- **Propósito**: Especifica a quién está destinado el token, es decir, quién debe aceptar y procesar el token.
- **Uso**: Al crear un JWT, el emisor puede incluir un reclamo `aud` (audiencia) que contiene una cadena o una lista de cadenas que identifican a los destinatarios previstos. Cuando un servicio recibe un token, debe verificar que su identidad coincide con uno de los valores de la audiencia.
- **Características**: Ayuda a garantizar que el token no sea utilizado por entidades no autorizadas.
- **Ejemplo**: `"my-api-users"`, `["service1", "service2"]`

#### Issuer (Emisor)
- **Propósito**: Indica quién emitió el token.
- **Uso**: Al crear un JWT, el emisor incluye un reclamo `iss` (emisor) que contiene una cadena que identifica al emisor del token. Cuando un servicio recibe un token, debe verificar que el emisor coincide con el valor esperado.
- **Características**: Ayuda a garantizar que el token provenga de una fuente confiable.
- **Ejemplo**: `"auth.mycompany.com"`

#### En resumen:
- **Secret Key** es una clave de seguridad usada para firmar y validar la autenticidad de los tokens.
- **Audience** es un reclamo en el token que especifica quién debe aceptar el token.
- **Issuer** es un reclamo en el token que identifica quién emitió el token.

## Creación de clases para JWT

Hay que tener en cuenta que la autenticación contiene dos pasos:
- Generación de un JWT para dárselo al usuario.
- Validación cada vez que se haga una solicitud del recurso por parte del usuario, siempre y cuando el recurso esté protegido.

A continuación se describen las clases que intervienen:

**Capa "CleanArchitecture.Domain"**

- Record `PasswordHash`: para almacenar en Bdd con un valor seguro el password.
- Clase `PasswordHash`: incluyendo `PasswordHash`.
- Interfaz `IUserRepository`: se incluye GetByEmailAsync.

**Capa "CleanArchitecture.Infrastructure"**
- Clase Configurations/`UserConfiguration`: configuración del password.
- Interfaz Repositories/`IUserRepository`: se incluye GetByEmailAsync.
- Clase Authentication/`JwtProvider`: encargada de crear el token.
- Clase Authentication/`JwtOptions`: encargada de recoger los settings que utilizará `JwtProvider`.

- **Capa "CleanArchitecture.Application"**
- Interfaz Authentication/`IJwtProvider`: contrato que implementa `JwtProvider`.
- Se inluye toda la configuración de LoginUser: Users/LoginUser:
	- `LoginCommand`, `LoginCommandHandler`, `LoginUserRequest`.

- **Capa "CleanArchitecture.Api"**
- Clase `JwtBearerOptionsSetup`, configura las opciones de autenticación JWT para la aplicación.
- Clase `UsersController`, necesario para conseguir un JWT. Debe ser `[AllowAnonymous]`.
- Clase `VehiculosController`, configura un método que solo se puede acceder si se tiene un Jwt válido. Debe ser `[Authorize]`.

# SECCIÓN 04. Seguridad y migración de EF en Clean architecture <a name="Seccion_04_Migracion"></a>

- Clase `UsersController`: se ha creado el método `Register` de usuarios, que espera:
`
{
  "email": "string",
  "nombre": "string",
  "apellidos": "string",
  "password": "string"
}
`

A su vez, usa las clases:
- `RegisterUserCommand.cs`
- `RegisterUserCommandHandler.cs`
- `RegisterUserCommandValidator.cs`
- `RegisterUserRequest.cs`

# SECCIÓN 06. Authorization con permisos y roles en Clean Architecture <a name="Seccion_06_Authorization"></a>

- Un buen sistema para el proceso de autorización del producto es el siguiente:
	- **Usuarios:** varios usuarios (en la carga inicial habrá al menos 2)
	- **Roles:**
		- Cliente: permiso de lectura.
		- Admin: todos los permisos.
	- **Permisos:** lectura, escritura, modificación.
	- Existirán tablas intermedias entre usuarios y roles y entre roles y permisos.

![My Image](./docs/imgs/10.Authorization1.JPG)

## Clases abstractas y genéricas para authorization

Para gestionar los roles y permisos, se han creado las siguientes clases:

**Clases genéricas:**
- Clase `Enumeration`: clase abstracta que se encarga de gestionar los enumerados.

**Clases para gestionar los roles:**
- Clase `Role`: clase que hereda de Enumeration y que se encarga de gestionar los roles.
- Clase `RoleConfiguration`: clase que se encarga de la configuración de los roles dentro de EF.
- Clase `UserRoleConfiguration`: clase que se encarga de la configuración de la tabla intermedia entre roles y usuarios dentro de EF.
- Dentro de la clase `User` se ha añadido la colección de roles.

**Clases para gestionar los permisos:**
- Clase `Permission`: clase que hereda de Entity y que se encarga de gestionar los permiso
- Clase `PermissionObjectValue`: colección de object values para los permisos.
- Clase `PermissionEnum`: enum que contiene los permisos.
- Dentro de la clase `Role` se ha añadido la colección de permisos.
- Clase `RolePermission`: clase que se encarga de la configuración de la tabla intermedia entre roles y permisos. en la clase `RoleConfiguration` se indica que un rol tiene una colección de permisos.
- Clase `RolePermissionConfiguration`:  clase que se encarga de la configuración de la tabla intermedia entre roles y permisos dentro de EF, y de insertar los datos intermedios.
- Clase `PermissionConfiguration`: clase que se encarga de la configuración de la tabla de permisos, y de insertar esos datos maestros.


# SECCIÓN 07. Authorization en Controllers y Json Web Tokens (JWT) <a name="Seccion_07_Authorization_Jwt"></a>

Esquema:

| ![My Image](./docs/imgs/11.AuthorizationJWT_1.PNG) | ![My Image](./docs/imgs/11.AuthorizationJWT_2.PNG) |
|:---------------------------------------------:|:---------------------------------------------:|
| ![My Image](./docs/imgs/11.AuthorizationJWT_3.PNG) | ![My Image](./docs/imgs/11.AuthorizationJWT_4.PNG) |


**Clases en CleanArchitecture.Infrastructure:**

- Clase `CustomClaims`: clase para gestionar los claims personalizados.
- Clase `HasPermissionAttribute`: representa un atributo de autorización para verificar si un usuario tiene un permiso específico.
- Clase `PermissionAuthorizationHandler`: representa un manejador de autorización para verificar si un usuario tiene un permiso específico.
- Clase `PermissionAuthorizationPolicyProvider`: proporciona una política de autorización personalizada basada en permisos.
- Clase `PermissionRequirement`: representa un requisito de autorización para verificar si un usuario tiene un permiso específico.
- La clase `JwtProvider` se ha modificado para que devuelva un token con los claims del usuario y sus permisos.

La migración a nivel de roles generará un esquema como el siguiente:

![My Image](./docs/imgs/11.AuthorizationJWT_5.PNG)

## ¿Cómo probar los cambios?

- Mediante postman, lanzar users/login. Si introducimos alguno de los usuarios y passwords de la aplicación:

| Email            | Password |
|------------------|----------|
| admin@aaa.com| Admin123$ |
| cliente@aaa.com| Test123$ |

El usuario conseguirá un token como el siguiente:

```eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJVc2VySWQgeyBWYWx1ZSA9IGZkYmRkYTNlLWRmNjgtNDc3Ny1hMTM5LTUxMWFkZmJjZmE0YyB9IiwiZW1haWwiOiJhZG1pbkBhYWEuY29tIiwicGVybWlzc2lvbnMiOlsiUmVhZFVzZXIiLCJXcml0ZVVzZXIiLCJVcGRhdGVVc2VyIl0sImV4cCI6MTc1MTYzMjc4MCwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo5MDAwIiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo5MDAwIn0.97QLlTLLQEEbv2dj8Zi0S5CibIcVnKx3_iJSST6G2nU```

Este token se puede visualizar en la web [https://jwt.io/](https://jwt.io/) con el siguiente formato:
![My Image](./docs/imgs/11.AuthorizationJWT_6.PNG)

Con ese token se podrá acceder a realizar una búsqueda en una consulta protegida por JWT (`[HasPermission(PermissionEnum.ReadUser)]`), como es el caso de `Search` de `Vehiculos`:

![My Image](./docs/imgs/11.AuthorizationJWT_7.PNG)

# SECCIÓN 08. Serilog en clean architecture y Net <a name="Seccion_08_Serilog"></a>

Esquema:
![My Image](./docs/imgs/12.Serilog_1.PNG)

Para poder utilizar Serilog, se han utilizado los siguientes paquetes Nuget:
- `Serilog`.
- `Serilog.AspNetCore`.

A través de Serilog, se guardan logs en:
- Consola.
- Ficheros de logs, con un fichero con todos los logs, y uno concreto para logs de errores.

La configuración de Serilog se realiza en el archivo `appsettings.json`.

Para poder utilizar Serilog, se han creado / modificado las siguientes clases:

Proyecto CleanArchitecture.Infrastructure:
- Clase `Program`: se ha modificado para que utilice Serilog.
- Clase `RequestContextLoggingMiddleware`: Este middleware agrega un identificador de correlación a cada solicitud HTTP para facilitar el seguimiento y la depuración.
- Clase `ApplicationBuilderExtensions`: se registra Serilog en el contenedor de dependencias.
- Clase `LoggingBehavior`: se ha modificado su comportamiento para que registre no solamente commands, sino también queries.

# SECCIÓN 09. Paginación en Clean Architecture <a name="Seccion_09_Paginacion"></a>

## Conceptos básicos de uso de paginación
- Es una técnica para administrar un volumen alto de datos.
- Se despliega solo una porción de los datos.
- Estas porciones se pasan en records.
- A cada grupo se le llama página.

![My Image](./docs/imgs/13.Paginacion_1.PNG)

## Paginación con el patrón Specification

Se le va a devolver al cliente:
```csharp
Count { get; } //Representa el número total de registros.
PageIndex { get; } //Representa el número de página.
PageSize { get; } //Representa cuántos records por grupos.
Data { get; } //Representa el dato en sí, los records que devuelve la consulta.
PageCount { get; } //Representa el número de páginas totales que se han generado.
int ResultByPage { get; } //Representa el número de records en una página concreta.
```

La intención es que cuando se lance una consulta, se devuelva un objeto con estos campos paginados, pero que además pueda filtrar los vehículos por modelos.

### ¿Cómo probar la paginación con el patrón Specification?

Los ejemplos se encuentran en la colección de Postman `CleanArchitecture.postman_collection.json`.

| ![My Image](./docs/imgs/13.Paginacion_2a.PNG) | ![My Image](./docs/imgs/13.Paginacion_2b.PNG) |
|:---------------------------------------------:|:---------------------------------------------:|

### ¿Qué clases se utilizan para la paginación con el patrón Specification?

Para realizar la paginación, se han creado / modificado las siguientes clases / interfaces:

**Capa CleanArchitecture.Domain:**
- `ISpecification.cs`: Interfaz que define las especificaciones de consulta para las entidades.
- `IVehiculoRepository.cs`: Interfaz que define las operaciones de consulta de vehículos y que contienen el patrón specification.
- `BaseSpecification.cs`: Clase base para las especificaciones de consulta mediante el patrón specification, indicando `Criteria`, `Includes`, `OrderBy`, `OrderByDescending`, `Take`, `Skip`, `IsPagingEnabled`.
- `PaginationResult.cs`: Clase de resultados que incluye paginación.
- `SpecificationEntry.cs`: Clase utilizada en las request, que representa una entrada de especificación de consulta.
- `VehiculoPaginationCountingSpecification.cs`: Clase de especificación para contar los vehículos.
- `VehiculoPaginationSpecification.cs`: Clase de especificación para paginar los vehículos en función de unos criterios especificados. Hereda de la clase `BaseSpecification`.

**Capa CleanArchitecture.Application:**
- `GetVehiculosByPaginationQuery.cs`: Clase que representa la consulta para obtener vehículos paginados.
- `GetVehiculosByPaginationQueryHandler.cs`: Clase para manejar la consulta de obtener vehículos por paginación.

**Capa CleanArchitecture.Infrastructure:*
- `SpecificationEvaluator.cs`: Clase que evalúa las especificaciones de consulta antes de enviarlas a la base de datos. Utiliza Entity Framework Core para aplicar los criterios, inclusiones, ordenamientos y paginación.
- `Repository.cs`: se añaden los siguientes métodos:
    - `ApplySpecification`: Método que aplica una especificación de consulta a una consulta de entidades.
    - `GetAllWithSpec`: Método que obtiene todas las entidades que cumplen con una especificación de consulta.
    - `CountAsync`: Método que obtiene la cantidad de entidades que cumplen con una especificación de consulta.

**Capa CleanArchitecture.Api:**

- `VehiculosController.cs': se añade el método `GetPaginationVehiculos` para obtener vehículos paginados.

# SECCIÓN 12. Versionado en Apis <a name="Seccion_12_Versionado"></a>

Las versiones en las APIs son fundamentales para mantener la estabilidad, la flexibilidad y la evolución de los servicios. Aquí te presentamos las razones más importantes para implementar versiones en tus APIs:
- **Compatibilidad hacia Atrás**: Las versiones permiten que los cambios en la API no rompan las aplicaciones que ya están en uso.
- **Evolución y Mejora de Funcionalidades**: Las versiones permiten introducir nuevas características o mejoras sin interrumpir el servicio actual.
- **Beneficio**: Los desarrolladores pueden iterar y mejorar la API a lo largo del tiempo.
- **Desarrollo y Pruebas de Nuevas Funcionalidades**: Las versiones facilitan el desarrollo y las pruebas de nuevas funcionalidades sin afectar la producción.
- **Gestión de Dependencias**: Permite a los desarrolladores especificar qué versión de la API usan sus aplicaciones.
- **Comunicación Clara de Cambios**: Las versiones proporcionan una manera clara de comunicar actualizaciones y cambios a los consumidores de la API.
- **Manejo de Errores y Depuración**: Las versiones ayudan a identificar y solucionar problemas en diferentes versiones de la API.
- **Facilita el Soporte y la Documentación**: La versión de la API es una parte clave de la documentación técnica y el soporte.

Para poder utilizar este versionado, se ha utilizado el siguiente paquetes Nuget:
- `Asp.Versioning.Mvc`: facilita la implementación y gestión de versiones en APIs web que utilizan el framework MVC
- `Asp.Versioning.Mvc`.ApiExplorer: facilita la integración con **Swagger** y que no produzca errores. 

Las técnicas más comunes para versionar APIS son:
1. Versionado a través de la **URL**. Es la usada en este ejemplo y la más común de las técnicas.
2. Versionado a través del **header**.
3. Versionado a través de los **query parameters**.

## Configuración del versionado

**Nota:** en este ejemplo solo se ha realizado en el controlador `UsersController.cs`. Realmente se debería hacer en todos los controladores y endpoints.

**Capa CleanArchitecture.Api:**

- `ApiSupportedVersions.cs`: clase que contiene todas las versiones soportadas (para no "hardcodear" los datos).
- `ConfigureSwaggerOptions.cs`: Configura las opciones de Swagger para versionado de la API.
- `UsersController.cs`: se ha añadido las versiones soportadas en cada endpoint.

## Utilización de Swagger para diferentes versiones

Por defecto si se utiliza versionado, y aunque los endpoints funcionan, en **Swagger** se produce un error:
![My Image](./docs/imgs/12_Versionado_1.PNG)

Se deberá configurar la clase `program.cs` de `CleanArchitecture.Api` para incluir las configuraciones especiales:

```csharp
builder
    .Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1);
        options.ReportApiVersions = true; //Añade en el header el número de versión
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddMvc()
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();
        foreach (var description in descriptions)
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    });
}

```

Una vez realizada la configuración, ya aparecerán todas las versiones soportadas y su descripción:

![My Image](./docs/imgs/12_Versionado_3.PNG)

### ¿Cómo probar el versionado?

Los ejemplos se encuentran en la colección de Postman `CleanArchitecture.postman_collection.json`.

![My Image](./docs/imgs/12_Versionado_2.PNG)

# SECCIÓN 15. Unit test en Clean architecture <a name="Seccion_15_Testing_Clean_Architecture"></a>

Recomendación de tipos de testing en un proyecto:

![My Image](./docs/imgs/15.Testing_1.PNG)

Se crearán los siguientes proyectos de test:
- `CleanArchitecture.Domain.UnitTests`
- `CleanArchitecture.Application.UnitTests`
- `CleanArchitecture.ArchitectureTests`

Todos ellos, tendrán los siguientes paquetes Nuget:
1. `FluentAssertions`: Proporciona una sintaxis fluida para escribir aserciones en pruebas unitarias, haciendo que el código de prueba sea más legible y expresivo.
2. `Microsoft.NET.Test.Sdk`: Es el paquete principal que contiene el soporte para ejecutar pruebas en .NET, gestionando el ciclo de vida de las pruebas y proporcionando las herramientas básicas para la ejecución de pruebas.
3. `xunit`: Es un marco de pruebas para .NET que permite escribir y ejecutar pruebas unitarias, con soporte para pruebas basadas en atributos y extensibilidad a través de extensiones.
4. `xunit.runner.visualstudio`: Proporciona la integración necesaria para que el test runner de xUnit se ejecute dentro del entorno de desarrollo de Visual Studio, permitiendo ejecutar pruebas y ver resultados en el Test Explorer de Visual Studio.

Por otro lado, habrá proyectos que utilicen:
1. `NSubstitute`: librería de mocking. Similar a `Moq`.
2. `NetArchTest.Rules`: para testing de arquitectura (cumplimiento de reglas técnicas).

# SECCIÓN 21. Reportes con Quest Pdf <a name="Seccion_21_Reportes"></a>

**Quest PDF** es una librería .Net para trabajar con archivos PDF, de tipo open source. Para más información visitar [QuestPDF](https://www.questpdf.com/).

**Nota:** es necesario revisar si la licencia es compatible con un uso empresarial.

El ejemplo generará un informe de vehículos, filtrado por modelo en el caso de que se informe.

![My Image](./docs/imgs/21_Reporte_1.PNG)

## Código implementado:

* Proyecto `CleanArchitecture.Api`:
    * Clase `AlquileresController.cs`: inclusión de una acción para el reporte llamada `ReporteVehiculos`:
    * Clase `program.cs`: este proceso quedará grabado en el código, en el `program.cs` del proyecto `CleanArchitecture.Api`:

```csharp
QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
```

* Proyecto `CleanArchitecture.Application`:
    * Clase `ReportVehiculoPdfQuery.cs`: Declaración de una consulta inmutable (record) para generar un reporte PDF de vehículos.
    * Clase `ReportVehiculoPdfQueryHandler.cs`: Clase que maneja la generación del PDF para el reporte de vehículos.

### ¿Cómo probar el reporte?

Los ejemplos se encuentran en la colección de Postman `CleanArchitecture.postman_collection.json`.

![My Image](./docs/imgs/21_Reporte_2.PNG)

**Nota:** desde Swagger no es posible probar, si se quiere probar directamente desde el navegador, habrá que introducir en el navegador directamente la URL, por ejemplo: 

`
http://localhost:9000/api/v1/vehiculos/reporte
`