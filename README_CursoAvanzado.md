# Clean architecture y Domain Driven Design - avanzado

Ejercicios tomados del curso de .Net University en Udemy: **Clean Architecture y Domain Driven Design en ASP.NET Core 8 avanzado**, y complementado con apuntes propios.
Para poder realizarlo, es necesario tener claro el curso anterior: **Clean Architecture y Domain Driven Design en ASP.NET Core 8**

---

# Índice completo de contenidos 📋
1. **[SECCIÓN 02. Domain Driven Design con Identificadores avanzados (strong ids)](#Seccion_02_StrongIds)**
2. **[SECCIÓN 03. Modelos de authentication en clean architecture](#Seccion_03_Authentication)**
3. **[SECCIÓN 04. Seguridad y migración de EF en Clean architecture](#Seccion_04_Migracion)**
4. **[SECCIÓN 06. Authorization con permisos y roles en Clean Architecture](#Seccion_06_Authorization)**
5. **[SECCIÓN 07. Authorization en Controllers y Json Web Tokens (JWT)](#Seccion_07_Authorization_Jwt)**
6. **[SECCIÓN 08. Serilog en clean architecture y Net](#Seccion_08_Serilog)**

---

## Agradecimientos 🎁

* Plataforma de aprendizaje online [Udemy](https://www.udemy.com/share/109PRS3@gz4ZDXhSu8i9pa_CnjiahHDgwCptf9vw-CYR0FqedgI2UGsgwy4nmPTe3ehw5QaGMA==/)
* A cualquiera que me invite a una cerveza 🍺.

---

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
- Serilog.
- Serilog.AspNetCore.

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

