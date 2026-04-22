
# 🚀 IntergalaxyTech API — .NET 10 (Clean Architecture)

Prueba Técnica — Lead .NET Developer

---

# Descripción del Proyecto

API REST construida en **.NET 10** para la gestión de personajes del universo Rick and Morty y solicitudes asociadas a eventos de producción.

El sistema implementa arquitectura limpia, EF Core, integración con API externa, validaciones, Docker y preparación para Azure Cloud.

---

## 📌 Nota de despliegue

Este proyecto fue desplegado utilizando **Railway** como plataforma de hosting, lo que permite integración continua desde el repositorio y despliegue automático del backend.

## 🌐 Base URL (Production)

La API está desplegada en Railway:
https://intergalaxy-production.up.railway.app/scalar


# 🏗️ Arquitectura

Se utiliza **Clean Architecture simplificada**:
DDD (Domain-Driven Design) para modelar el dominio de personajes y solicitudes, con capas claramente definidas:
CQRS (Commands/Queries) y Result Pattern para manejo de errores.

```
src/
 ├── Intergalaxy.Api
 ├── Intergalaxy.Application
 ├── Intergalaxy.Domain
 ├── Intergalaxy.Infrastructure

tests/
 ├── Intergalaxy.Tests
 [TODO] command testing...
```

## Technologies

* [ASP.NET Core 10](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core)
* [Aspire](https://aspire.dev)
* [Entity Framework Core 10](https://docs.microsoft.com/en-us/ef/core/)
* [MediatR](https://github.com/jbogard/MediatR)
* [AutoMapper](https://automapper.org/)
* [FluentValidation](https://fluentvalidation.net/)
* [NUnit](https://nunit.org/), [Shouldly](https://docs.shouldly.org/), [Moq](https://github.com/devlooped/moq) & [Respawn](https://github.com/jbogard/Respawn)
* [Scalar](https://scalar.com/)
* .NET Aspire para orquestación y observabilidad local
* Azure Key Vault para gestión de secretos
* Docker para contenerización
* OpenAPI para documentación
* SQLite para persistencia local

### Principios aplicados

* Health Checks para monitoreo
* Clean Architecture para mantenibilidad y escalabilidad
* DDD para modelado de dominio
* CQRS para separación de comandos y consultas
* Result Pattern para manejo de errores explícito
* DTOs para desacoplar API de dominio
* AsNoTracking para optimización de consultas de solo lectura
* HttpClientFactory para integración externa con resiliencia
* FluentValidation para validación de entrada robusta
* Separación de responsabilidades
* Dependency Inversion Principle
* DTOs para comunicación externa
* Dominio aislado de infraestructura

---


# Configuración de Secrets (Azure Key Vault)

La aplicación está preparada para usar **Azure Key Vault** en producción.

```json
{
  "KeyVault": {
    "Url": "https://intergalaxy-kv.vault.azure.net/"
  }
}
```

Las connection strings se almacenan como secretos en Key Vault:

```
ConnectionStrings--Default
```

---

# Endpoints

## Personajes

| Método | Endpoint                 | Descripción                          |
| ------ | ------------------------ | ------------------------------------ |
| POST   | /api/Characters/importar | Importa personajes desde API externa |
| GET    | /api/Characters          | Listado paginado con filtros         |
| GET    | /api/Characters/{id}     | Detalle de personaje                 |

---

## Solicitudes

| Método | Endpoint                     | Descripción               |
| ------ | ---------------------------- | ------------------------- |
| POST   | /api/CharacterRequests             | Crear solicitud     | [TODO]
| GET    | /api/CharacterRequests             | Listado con filtros | [TODO]
| GET    | /api/CharacterRequests/{id}        | Detalle             | [TODO]
| PATCH  | /api/CharacterRequests/{id}/estado | Cambio de estado    | [TODO]

---

## Health Check
> ServiceDefaults.Extensions

```http
GET /health
```

Respuesta:

```json
{
  "status": "Healthy",
  "database": true,
  "timestamp": "2026-01-01T00:00:00Z"
}
```

---

# Reglas de Negocio

* Solicitudes solo pueden crearse si el personaje existe
* Un personaje solo puede estar en un evento aprobado a la vez
* Estados válidos:

  * Pending → InProgress
  * InProgress → Approved / Rejected
  * Pending → Rejected
* Validación obligatoria de campos

---

# Base de Datos (EF Core)

Migraciones:

```bash
# Crear migración
dotnet ef migrations add InitialCreate

# Aplicar base de datos
dotnet ef database update
```

---
# Docker

## Build

```bash
docker build -t intergalaxy-api .
```

## Run

```bash
docker run -p 8080:80 intergalaxy-api
```

## Docker Compose

```yaml
version: '3.9'
services:
  api:
    build: .
    ports:
      - "8080:80"
    environment:
      - ConnectionStrings__Default=Data Source=app.db
```

---
# Respuestas sobre Diseño Azure

| Necesidad del sistema      | Servicio Azure que usarías | Razón                                                                   |
| -------------------------- | -------------------------- | ----------------------------------------------------------------------- |
| Hospedar la API .NET 8     | Azure App Service          | Hosting PaaS con despliegue simple, escalado automático y health checks |
| Base de datos relacional   | Azure SQL Database         | Servicio administrado con alta disponibilidad y backups automáticos     |
| Almacenar PDFs o archivos  | Azure Blob Storage         | Almacenamiento escalable y económico para archivos no estructurados     |
| Exponer y versionar API    | Azure API Management       | Control de versiones, seguridad, rate limiting y documentación          |
| Tareas programadas / async | Azure Functions            | Serverless, ideal para jobs y procesos desacoplados                     |

---

# 🧾 Análisis de código legado

## ❌ Problemas identificados

* SQL Injection por concatenación
* Credenciales hardcodeadas
* Sin separación de capas
* Uso de ADO.NET manual
* Validación en UI
* Uso de Session state
* Sin manejo de errores

---

##  Versión moderna (POST /api/solicitudes)

* EF Core en lugar de SQL manual
* Validación con FluentValidation
* DTOs desacoplados
* Manejo de errores centralizado
* Arquitectura en capas

---
#  Preguntas de Liderazgo Técnico

##  Migración del sistema legado en etapas graduales

Planificaría una migración incremental usando el patrón **Strangler Fig**:

* Identificar dominios funcionales (Personajes, Solicitudes)
* Exponer una nueva API .NET 8 paralela al sistema legado
* Migrar endpoints por módulo, comenzando por los menos críticos
* Mantener sincronización de datos durante la transición (dual write o eventos)
* Retirar progresivamente funcionalidades del sistema legado
* Validar cada fase con pruebas funcionales y de integración

---

## 🔄 Estrategia para operación en paralelo

Durante la coexistencia de sistemas:

* Usar un **API Gateway o routing layer** para dirigir tráfico
* Mantener sincronización entre bases de datos (temporalmente si es necesario)
* Implementar **feature flags** para habilitar nuevas rutas gradualmente
* Usar eventos o colas (Azure Service Bus) para consistencia eventual
* Monitorear ambos sistemas con observabilidad centralizada (logs + metrics)

---

## Organización del equipo (3 desarrolladores)

### Roles

* 👨‍💻 Dev 1: Domain & Core Logic (DDD, entidades, reglas de negocio)
* 👨‍💻 Dev 2: Infrastructure & Integrations (EF Core, APIs externas, Azure)
* 👨‍💻 Dev 3: API & Quality (Controllers, Swagger, testing, validation)

---

### Git Strategy

* `main`: producción
* `develop`: integración
* feature branches:

  * `feature/personajes-module`
  * `feature/solicitudes-module`

Pull Requests obligatorios con code review cruzado entre los desarrolladores

---

### Code Review

* Validación de arquitectura (no lógica en controllers)
* Revisión de performance (AsNoTracking, queries eficientes)
* Validación de estándares (naming, DTOs, validaciones)
* Revisión de seguridad (inyección SQL, secretos)

---
---

# Uso de IA

Este proyecto fue desarrollado con asistencia de:

* ChatGPT (arquitectura, refactor y documentación)
* GitHub Copilot (auto-completado de código)

---

# Decisiones técnicas

* Clean Architecture para escalabilidad
* EF Core para persistencia moderna
* HttpClient para integración externa
* Result Pattern para control de errores
* AsNoTracking para performance

---

# Preparación Azure

* App Service ready
* Key Vault integration
* Health checks para monitoring
* Configuración por variables de entorno

---

# Notas finales

* El sistema está diseñado como API cloud-native
* Preparado para despliegue en Azure
* Enfocado en mantenibilidad y escalabilidad

